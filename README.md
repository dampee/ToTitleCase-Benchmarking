# ToTitleCase-Benchmarking
Because James asked if there is a faster way. https://twitter.com/James_M_South/status/1161638705519448065?s=20
I am by no means a performance hacker, but after a few minutes (and a second try) I can say: yes there are faster ways than the [TextInfo.ToTitleCase](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.textinfo.totitlecase?view=netframework-4.8)

// * Summary *

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17134.885 (1803/April2018Update/Redstone4)
Intel Core i7-6700K CPU 4.00GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=3914061 Hz, Resolution=255.4891 ns, Timer=TSC
.NET Core SDK=2.2.401
  [Host]     : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT


|                   Method |            TitleCase |     Mean |      Error |     StdDev |
|------------------------- |--------------------- |---------:|-----------:|-----------:|
|      TextInfoToTitleCase | snow (...)warfs [31] | 480.9 ns |  5.1896 ns |  4.6005 ns |
|                WithChars | snow (...)warfs [31] | 623.4 ns |  6.4966 ns |  6.0769 ns |
| WithCharsCheckingTolower | snow (...)warfs [31] | 263.4 ns |  2.0757 ns |  1.9416 ns |
|      TextInfoToTitleCase | the l(...)icans [24] | 388.6 ns |  3.8549 ns |  3.6058 ns |
|                WithChars | the l(...)icans [24] | 490.1 ns | 13.5846 ns | 12.7071 ns |
| WithCharsCheckingTolower | the l(...)icans [24] | 184.8 ns |  0.9123 ns |  0.7618 ns |
