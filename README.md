# Obstgarten

A small .NET 8 simulation project for exploring probabilities and strategies in the children's cooperative board game **Obstgarten** (Orchard).

The program models complete games, runs many independent simulations in parallel, and evaluates how often the players manage to collect all fruit before the raven is completed.

## What the code does

The core simulation is implemented by the generic `Game<T>` class. A game keeps track of:

- the fruit remaining on each tree,
- the number of raven pieces already laid,
- the number of turns played,
- the last dice result,
- the dice implementation used for the game, and
- the strategy used when a basket/joker result allows the players to choose fruit.

A turn asks the configured `IDice<T>` for the next result. Depending on that result, the game either adds a raven piece, removes fruit directly, or delegates the fruit choice to an `IChoseFruitsStrategy<T>` implementation. The game ends when either all fruit has been collected or all raven pieces have been laid.

The model is generic over an enum type, so dice faces and game colours are not hard-coded into the game engine itself. The standard game configuration uses the values defined by `GameParameters.DefaultColors`.

## Simulation

`Program.cs` currently runs a Monte Carlo-style simulation of many games. Each simulated game gets its own `DefaultDice` instance and is played until completion. The simulations are executed with `Parallel.For` using the available processor cores, while results are collected in a thread-safe `ConcurrentBag`.

The current executable uses `ChoseOfMostRemainingFruitsStrategy` and finally prints the percentage of games won by the players.

This makes it easy to change rules or strategies and compare their effect on the probability of winning.

## Strategies

Fruit selection for joker/basket rolls is separated from the game logic through `IChoseFruitsStrategy<T>`. The repository contains several strategy implementations, including strategies that prefer fruit types with the largest remaining counts and strategies based on fixed favourites.

This separation allows alternative player behaviour to be tested without changing the simulation engine.

## Dice abstraction

Dice behaviour is abstracted through `IDice<T>`. `DefaultDice<T>` provides the normal randomized implementation used by the simulation. Because the game depends on the interface rather than directly on a random-number generator, alternative or deterministic dice implementations can also be supplied, which is particularly useful for testing.

## Project structure

```text
Obstgarten.sln
├── Obstgarten/
│   ├── Dices/          Dice abstractions and default implementation
│   ├── Game/           Game state, rules, parameters and result interfaces
│   ├── Statistics/     Result/statistics support used by simulations
│   ├── Strategies/     Pluggable fruit-selection strategies
│   └── Program.cs      Parallel simulation entry point
├── UnitTests/          Automated tests for game behaviour
└── Statistics/         Additional analysis data (currently an Excel workbook)
```

## Technology

The main application is a C# console application targeting **.NET 8** with nullable reference types and implicit usings enabled.

## Running the simulation

With the .NET 8 SDK installed:

```bash
dotnet run --project Obstgarten/Obstgarten.csproj
```

The application runs the configured number of games and reports the resulting player win percentage.

Tests can be run with:

```bash
dotnet test Obstgarten.sln
```

## Purpose

This is a private-for-fun project intended to explore the probabilities behind *Obstgarten* by direct simulation and to experiment with how different game parameters and player strategies influence the outcome.
