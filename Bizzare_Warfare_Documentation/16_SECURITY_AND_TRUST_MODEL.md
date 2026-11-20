# 16 — Security & Trust Model
## Goal
Casual LAN security with fundamental authority and validation. V1 is not competitive esports anti-cheat.

## Trust
Host is authoritative; clients are untrusted. Host validates firing requests, ammo, cooldown, damage, score, state transitions, match configuration and session identity.

## Client must not directly set
Health, score, another player's health, ammo, match winner, timer or bot state.

## Abuse controls
Input validation, RPC permission checks, message size limits, rate limiting, connection caps, invalid-state rejection and timeout handling.

## Limitation
Listen-server host can manipulate its own authoritative process. V1 does not protect against malicious host. Competitive future version may require dedicated servers and stronger integrity/anti-cheat systems.

## Privacy
No accounts or unnecessary personal data in V1. Any future telemetry requires separate documented collection and consent approach.
