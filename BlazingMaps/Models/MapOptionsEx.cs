using FisSst.BlazorMaps;
using System.Diagnostics.CodeAnalysis;

namespace BlazingMaps.Models;

[ExcludeFromCodeCoverage]
public class MapOptionsEx : MapOptions
{
    public int ZoomOnSelection { get; set; } = 17;
}
