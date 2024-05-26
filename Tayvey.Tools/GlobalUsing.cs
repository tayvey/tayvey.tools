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
global using System.Text.Json.Serialization;

// NUGET
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.OpenApi.Models;
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization.Attributes;
global using MongoDB.Driver;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Serialization;
global using OfficeOpenXml;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using System.ComponentModel;

// 项目
global using Tayvey.Tools.TvApiResults.Enums;
global using Tayvey.Tools.TvApiResults.Models;
global using Tayvey.Tools.TvAutoDIs;
global using Tayvey.Tools.TvAutoDIs.Attrs;
global using Tayvey.Tools.TvAutoDIs.Enums;
global using Tayvey.Tools.TvConfigs;
global using Tayvey.Tools.TvConfigs.Models;
global using Tayvey.Tools.TvExcels.Models;
global using Tayvey.Tools.TvMongos.Attrs;
global using Tayvey.Tools.TvMongos.Models;
global using Tayvey.Tools.TvSockets.Enums;
global using Tayvey.Tools.TvSockets.Models;
global using Tayvey.Tools.TvSwaggers.Models;
#endif