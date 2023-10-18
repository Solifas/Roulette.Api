This Document Serves to give information about the Roulette API Codebase

This project is written in .NET 5
Pattern used: CQRS
Assumptions
 - There are only Betting types of the game and the spin function only randomly selects the winning betting type with a thread delay set on      the AppSettingsFile file
 - Based on the outcome of the spin the bets made by users are updated
     - the user balance is also then updated if they have a winning bet

  My understanding of the Game notes

  
  - Table minimum and max
    - Outside min max
    - Inside min max

- Bet on black - ball lands on black
    - Bet on red vice versa

- Bet on Odd : Ball lands on an odd number
    - Bet on Even  Vice versa

- 1 to 18 - any number from 1- 18
    - 19 to 36: any number from 19-36

Dozens
- 1st 12 any number  between 1-12
- 2nd 12 any number between 12-24
- 3rd 12 any number between 25 and 34
Columns
- 3,6,9,12,15,18,21,24,27,30,33,36
- 2,5,8,10,14,17,20,23,26,29,32,35
- 1,4,7,10,13,16,19,22,25,28,31,34

Outside can’t bet more than 1 bet

Inside -  bet on any one number pays 35-1
Split - bet on 2 numbers pays 17-1
Corner bet on 4 numbers pays 8-1
Street: bet on 3 number along a row pays 11-1
Double street bet on 2 row of 6 numbers pays 5-1
TopLine 5 number bet 0,00,1,2,3 pays 6 to 1

There is a split of 00&1 or 00&2 or 0&3 or 0&2 pays 17-1
Trio between 00,1&2 or 0,2&23 pays 11-1 // Basket is the same 0,00&2

Bet types that can come out from a spin:
        Single,
        Split
        Corner,
        Street,
        DoubleStreet,
        TopLine,
        Trio,
        Black,
        Red,
        Odd,
        Even,
        FirstHalf,
        SecondHalf,
        FirstDozen,
        SecondDozen,
        ThirdDozen,
        FirstColumn,
        SecondColumn,
        ThirdColumn

Challenges:
- String not casting to Guid
    - Temporary solution Update all Guids to String (Can do better Solie) :(
    
