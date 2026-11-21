# Technical design — api

## Proposed implementation

ASP.NET Core controllers validate the request and actor. EF Core persists records in Neon PostgreSQL. Propose a unique key on learner and client attempt ID. Store the rubric version and decisions in the same transaction. On repeated IDs compare the payload before returning the original result. Reject conflicting content. Scope teacher queries by assignment and current membership.

## Verification

Repeat a request after losing its response. Expect one confirmed attempt. Submit one of two attempts and verify only that submission appears for the assigned teacher. Record endpoint, role, expected status, database fixture, and observed row count.
