# DECISIONS.md — AI Usage Log

##  (Assignment 10 — Three Enemy Types)

- Used Claude (AI agent) to write the initial C# scripts for all three enemy types: EnemyPatroller (Patrol→Chase→Attack→Cooldown), EnemyShooter (Idle→Aim→Fire→Cooldown), EnemyCharger (Idle→WindUp→Charge→Recover). I set up all the GameObjects, waypoints, layers, and sprites myself in the Unity Editor.
- Mistake found: the assignment requires enemies to check distance, angle AND line-of-sight, but the first version only checked distance + raycast, no field-of-view angle. Caught this by comparing the code against the assignment checklist and fixed it by adding a `visionAngle` field + `Vector2.Angle()` check to all three scripts.
- Mistake found: EnemyShooter and EnemyCharger had a facing-direction bug — the enemy could only "see" the player from one side because flipX was being set automatically only after the player was already detected (circular dependency). Fixed by setting Flip X manually in the Inspector for stationary enemies instead of computing it automatically.
- Mistake found: EnemyShooter was firing bullets in the opposite direction from where it was facing. Fixed the `dir` sign in FireState() to match the facing vector used in detection.
- Mistake found: accidentally staged the entire iOSBuild/ folder (4700+ generated Xcode files) into a git commit. Caught it before pushing, fixed by `git reset --soft`, adding `iOSBuild/` to `.gitignore`, and re-committing only the actual script/scene changes.
- I reviewed and understand every method in EnemyPatroller.cs, EnemyShooter.cs, EnemyCharger.cs, and EnemyBullet.cs — I can explain the FSM states, the raycast/angle vision check, and the wind-up/cooldown timing for each enemy at the defense.

## Playtesting

- Playtested with 2 outside testers on my iPhone (real device, not simulator).
- Tester 1: 4 tries to complete the level. Tester 2: 2 tries to complete the level.
- Tuning changes made after playtest (old → new):
    - visionAngle on EnemyShooter: 100 → 160 — reason: tester said the enemy didn't seem to react to the player at all; the detection cone was too narrow, so the enemy rarely noticed the player even when walking right past it.
    - windUpTime on EnemyCharger: 0.7 → 1.0 — reason: the charge attack's red warning flash went by too quickly for tester to react to, making the attack feel sudden/unfair. Lengthening the wind-up gives more visible warning time before the charge.
- Both testers found the Patroller and Shooter enemies fair after the visionAngle fix; the Charger needed the wind-up adjustment to feel readable.