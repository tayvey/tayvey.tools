#if NET6_0_OR_GREATER
// 框架
global using System.Collections.Concurrent;
global using System.Diagnostics;
global using System.Net;
global using System.Net.Sockets;
global using System.Reflection;
global using System.Runtime.InteropServices;
global using System.Security.Cryptography;
global using System.Text;

// NUGET
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Serialization;
global using OfficeOpenXml;

// 项目
global using Tayvey.Tools.TvApiResults.Enums;
global using Tayvey.Tools.TvApiResults.Models;
global using Tayvey.Tools.TvAutoDIs.Attrs;
global using Tayvey.Tools.TvAutoDIs.Enums;
global using Tayvey.Tools.TvConfigs.Models;
global using Tayvey.Tools.TvExcels.Models;
global using Tayvey.Tools.TvSockets.Enums;
global using Tayvey.Tools.TvSockets.Models;
#endif