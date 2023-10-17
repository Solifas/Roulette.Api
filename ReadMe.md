This Document Serves to give information about the Roulette API codebase

This project is written in .NET 5
Pattern used: CQRS
Assumptions
 - There are only Betting types of the game and the spin function only randomly selects the winning betting type with a thread delay set on      the AppSettingsFile file
 - Based on the outcome of the spin the bets made by users are updated
     - the user balance is also then updated if they have a winning bet
