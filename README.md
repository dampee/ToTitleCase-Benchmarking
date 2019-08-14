# ToTitleCase-Benchmarking
Because James asked if there is a faster way. https://twitter.com/James_M_South/status/1161638705519448065?s=20
I am by no means a performance hacker, but after a few minutes (and a second try) I can say: yes there are faster ways than the [TextInfo.ToTitleCase](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.textinfo.totitlecase?view=netframework-4.8).

## Results

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17134.885 (1803/April2018Update/Redstone4)
Intel Core i7-6700K CPU 4.00GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=3914061 Hz, Resolution=255.4891 ns, Timer=TSC
.NET Core SDK=2.2.401
  [Host]     : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT


|                           Method |            TitleCase |       Mean |     Error |    StdDev |
|--------------------------------- |--------------------- |-----------:|----------:|----------:|
|              TextInfoToTitleCase | snow (...)warfs [31] |   506.6 ns |  3.454 ns |  3.062 ns |
|                        WithChars | snow (...)warfs [31] |   656.1 ns |  8.457 ns |  7.911 ns |
|         WithCharsCheckingTolower | snow (...)warfs [31] |   280.4 ns |  3.738 ns |  3.313 ns |
|                    WithSubString | snow (...)warfs [31] | 1,146.3 ns |  8.859 ns |  7.854 ns |
|          WithRegexAndCultureInfo | snow (...)warfs [31] | 5,201.1 ns | 94.088 ns | 83.406 ns |
| WithCharsAndStringBuilderAndMore | snow (...)warfs [31] | 1,364.6 ns | 27.206 ns | 26.720 ns |
|              TextInfoToTitleCase | the l(...)icans [24] |   403.4 ns |  8.950 ns |  9.191 ns |
|                        WithChars | the l(...)icans [24] |   508.6 ns |  5.459 ns |  4.558 ns |
|         WithCharsCheckingTolower | the l(...)icans [24] |   204.2 ns |  2.652 ns |  2.351 ns |
|                    WithSubString | the l(...)icans [24] |   957.0 ns | 16.225 ns | 14.383 ns |
|          WithRegexAndCultureInfo | the l(...)icans [24] | 4,241.5 ns | 31.630 ns | 29.587 ns |
| WithCharsAndStringBuilderAndMore | the l(...)icans [24] | 1,085.1 ns | 13.073 ns | 12.228 ns |