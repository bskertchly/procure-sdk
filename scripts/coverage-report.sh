#!/bin/bash

# Coverage Report Generation Script
# Generates unified coverage reports across all test projects

set -e

# Configuration
COVERAGE_DIR="./coverage"
REPORT_DIR="./coverage-report"
MIN_COVERAGE=80
WARN_COVERAGE=85

# Colors for output
RED='\033[0;31m'
YELLOW='\033[1;33m'
GREEN='\033[0;32m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}📊 Procore SDK - Coverage Report Generation${NC}"
echo "============================================="

# Clean previous results
echo "🧹 Cleaning previous coverage results..."
rm -rf "$COVERAGE_DIR" "$REPORT_DIR"
mkdir -p "$COVERAGE_DIR" "$REPORT_DIR"

# Run tests with coverage for all test projects
echo -e "\n🧪 Running tests with coverage collection..."

# Find all test projects
TEST_PROJECTS=$(find tests -name "*.csproj" -path "*/tests/*" | grep -E "\.(Tests|Test)\.csproj$")

if [ -z "$TEST_PROJECTS" ]; then
    echo -e "${RED}❌ No test projects found${NC}"
    exit 1
fi

echo "Found test projects:"
for project in $TEST_PROJECTS; do
    echo "  • $(basename "$project" .csproj)"
done

# Run tests for each project
TOTAL_TESTS=0
FAILED_TESTS=0
COVERAGE_FILES=""

for project in $TEST_PROJECTS; do
    PROJECT_NAME=$(basename "$project" .csproj)
    echo -e "\n🔍 Testing $PROJECT_NAME..."
    
    # Create project-specific coverage directory
    PROJECT_COVERAGE_DIR="$COVERAGE_DIR/$PROJECT_NAME"
    mkdir -p "$PROJECT_COVERAGE_DIR"
    
    # Run tests with coverage
    if dotnet test "$project" \
        --configuration Release \
        --collect:"XPlat Code Coverage" \
        --results-directory "$PROJECT_COVERAGE_DIR" \
        --logger "trx;LogFileName=${PROJECT_NAME}.trx" \
        --verbosity minimal \
        -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura; then
        
        echo -e "  ${GREEN}✅ Tests passed${NC}"
        
        # Find and rename coverage file
        COVERAGE_FILE=$(find "$PROJECT_COVERAGE_DIR" -name "coverage.cobertura.xml" | head -1)
        if [ -n "$COVERAGE_FILE" ]; then
            NEW_COVERAGE_FILE="$COVERAGE_DIR/${PROJECT_NAME}.cobertura.xml"
            cp "$COVERAGE_FILE" "$NEW_COVERAGE_FILE"
            COVERAGE_FILES="$COVERAGE_FILES;$NEW_COVERAGE_FILE"
            echo -e "  ${GREEN}📋 Coverage collected${NC}"
        else
            echo -e "  ${YELLOW}⚠️ No coverage file found${NC}"
        fi
    else
        echo -e "  ${RED}❌ Tests failed${NC}"
        FAILED_TESTS=$((FAILED_TESTS + 1))
    fi
    
    TOTAL_TESTS=$((TOTAL_TESTS + 1))
done

# Check if we have coverage files
if [ -z "$COVERAGE_FILES" ]; then
    echo -e "\n${RED}❌ No coverage data collected${NC}"
    exit 1
fi

# Remove leading semicolon
COVERAGE_FILES=${COVERAGE_FILES#;}

echo -e "\n📈 Generating coverage reports..."

# Check if ReportGenerator is available
if ! command -v reportgenerator &> /dev/null; then
    echo "📦 Installing ReportGenerator..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
fi

# Generate HTML report
echo "🌐 Generating HTML report..."
reportgenerator \
    -reports:"$COVERAGE_FILES" \
    -targetdir:"$REPORT_DIR" \
    -reporttypes:"HtmlInline_AzurePipelines;Cobertura;JsonSummary;TextSummary" \
    -verbosity:Info \
    -title:"Procore SDK Coverage Report" \
    -tag:"$(git rev-parse --short HEAD 2>/dev/null || echo 'local')"

# Extract overall coverage percentage
if [ -f "$REPORT_DIR/Summary.json" ]; then
    # Try to extract coverage from JSON summary
    COVERAGE_PERCENT=$(grep -o '"linecoverage": *[0-9.]*' "$REPORT_DIR/Summary.json" | grep -o '[0-9.]*' | head -1)
    
    if [ -z "$COVERAGE_PERCENT" ]; then
        # Fallback to text summary
        if [ -f "$REPORT_DIR/Summary.txt" ]; then
            COVERAGE_PERCENT=$(grep "Line coverage:" "$REPORT_DIR/Summary.txt" | grep -o '[0-9.]*%' | head -1 | tr -d '%')
        fi
    fi
fi

# Report results
echo -e "\n📊 Coverage Report Summary"
echo "=========================="
echo -e "Test Projects: ${TOTAL_TESTS}"
echo -e "Failed Projects: ${FAILED_TESTS}"

if [ -n "$COVERAGE_PERCENT" ]; then
    COVERAGE_INT=${COVERAGE_PERCENT%.*}  # Remove decimal part
    
    echo -e "Overall Coverage: ${COVERAGE_PERCENT}%"
    
    # Evaluate coverage thresholds
    if [ "$COVERAGE_INT" -lt "$MIN_COVERAGE" ]; then
        echo -e "${RED}❌ Coverage ${COVERAGE_PERCENT}% is below minimum threshold of ${MIN_COVERAGE}%${NC}"
        COVERAGE_EXIT_CODE=1
    elif [ "$COVERAGE_INT" -lt "$WARN_COVERAGE" ]; then
        echo -e "${YELLOW}⚠️ Coverage ${COVERAGE_PERCENT}% is below recommended threshold of ${WARN_COVERAGE}%${NC}"
        COVERAGE_EXIT_CODE=0
    else
        echo -e "${GREEN}✅ Coverage ${COVERAGE_PERCENT}% meets quality standards${NC}"
        COVERAGE_EXIT_CODE=0
    fi
else
    echo -e "${YELLOW}⚠️ Could not determine overall coverage percentage${NC}"
    COVERAGE_EXIT_CODE=0
fi

# Report file locations
echo -e "\n📁 Generated Files:"
echo "  • HTML Report: $REPORT_DIR/index.html"
echo "  • Cobertura XML: $REPORT_DIR/Cobertura.xml"
echo "  • JSON Summary: $REPORT_DIR/Summary.json"

if [ -f "$REPORT_DIR/index.html" ]; then
    echo -e "\n🌐 Open the HTML report:"
    echo "  file://$(pwd)/$REPORT_DIR/index.html"
fi

# Final exit code
if [ "$FAILED_TESTS" -gt 0 ]; then
    echo -e "\n${RED}❌ Some tests failed${NC}"
    exit 1
elif [ "$COVERAGE_EXIT_CODE" -ne 0 ]; then
    echo -e "\n${RED}❌ Coverage below minimum threshold${NC}"
    exit 1
else
    echo -e "\n${GREEN}✅ All checks passed${NC}"
    exit 0
fi