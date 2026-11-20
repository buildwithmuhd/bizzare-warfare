# 07 — Networking Specification
## Model
LAN listen-server. Host acts as authoritative server; clients send input intent and render replicated state. Proposed transport: ENet; proposed port: 7777. Validate these defaults during prototype.

## Host authority
Owns match lifecycle, player spawning, bot logic, damage, score, ammo/fire-rate validation, timer and results.

## Connection flow
Client selects LAN discovery or direct IP → request connection → host validates → session/player state created → match state synchronized → spawn.

## Delivery semantics
Use reliable delivery for connection/lobby/match events and results. Use appropriate unreliable or unreliable-ordered delivery for frequent state snapshots where newer data supersedes old data.

## Data concepts
PlayerInput: movement, jump/sprint/fire intent, aim direction.
PlayerState: ID, position/rotation, health, ammo, score/deaths.
MatchState: map/mode, remaining time, scores, match status.

## Synchronization
Start with a simple host-authoritative state replication and interpolation. Add prediction only if profiling/playtesting shows latency needs it. Never let clients directly overwrite health, score, ammo or match result.

## Disconnects
Remove/deactivate entity, update lobby/scoreboard and notify peers. Host disconnect ends V1 session; host migration excluded.
