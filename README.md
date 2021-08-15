# Azenix code challenge

### Requirements
- .NET Core 3.1

### Projects
- HttpLogStatisticsGenerator contains the main code, Startup is the entry point
- HttpLogStatisticsGenerator.Tests contains unit tests written with NUnit, Moq and FluentAssertions

### Features
- Decoupled, pluggable components by following SOLID principles
  - S - Each class is responsible for a single piece of the overall logic
  - O - Can add and remove statistic generators by implementing the IStatisticsGenerator
  - L - The Orchestrator will receive several IStatisicGenerator; it's not concerned with the implementation, only with the StatisticResult
  - I - All classes are injected via an interface which have specific methods for that responsibility
  - D - Use of Microsoft's IoC container for dependency inversion
 
- Different Input methods can be used if this were e.g. moved into the cloud
- By having a RawHttpLogEntry and HttpLogEntryDto, the raw representation is independent (more useful in a database scenario to separate data storage representation from in memory representation)
- Configuration file appSettings.json to provide input file and Logging configuration
- Serilog logging has been provided

### Notes
- The HttpLogEntryDto just has two properties for the purposes of this exercise
- But the HttpLogParser and this Dto can be augmented to facilitate parsing of the remaining piece of information
- The list is enumerated multiple times for each statistic generator
  - If the dataset were larger or the statistic was more complex, then an inverted in-memory index (or use of Redis) can be implemented where multiple key properties would point to HttpLogEntryDto instances
