global using Xunit;
global using FluentAssertions;
global using NSubstitute;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Http;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using System;
global using System.Collections.Generic;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Net.Http;
global using Procore.SDK.Shared.Authentication;
// Temporarily disabled due to compilation issues with generated Kiota clients
// global using Procore.SDK.Core.Models;
global using Procore.SDK.Extensions;