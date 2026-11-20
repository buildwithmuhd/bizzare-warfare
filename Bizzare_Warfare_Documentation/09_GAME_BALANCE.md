# 09 — Game Balance
## Principles
Balance via playtesting and measured data. Preserve weapon identity, readable movement and fair spawn experiences.

## Weapon metrics
Damage, rate of fire, time-to-kill, magazine capacity, reload time, effective range, recoil and accuracy. Approximate `TTK = (shots_required - 1) / shots_per_second` assuming first shot lands immediately.

## Movement metrics
Walk/sprint speed, acceleration, deceleration, jump height, air control and friction. Movement should feel responsive while remaining trackable.

## Bot tuning
Tune reaction, accuracy, target switching, aggression and navigation. Avoid making difficulty merely higher damage.

## Map evaluation
Spawn fairness, cover, sightlines, verticality, navigation complexity, combat density and camping risks.

## Telemetry for internal tests
Average eliminations/deaths, match length, weapon use/accuracy, bot-human performance and spawn-related deaths. Document balance changes and compare builds.
