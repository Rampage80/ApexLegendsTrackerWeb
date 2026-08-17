# ApexLegendsTracker Product Roadmap

## Product vision
ApexLegendsTracker should become a public-facing dashboard and player insight experience for Apex Legends players. The product should help users quickly understand the live-state of the game and their own performance progression, without requiring authentication.

The initial product experience is:
- a public dashboard showing live Apex information
- a player search flow by player name and platform
- drill-down metrics tied to account performance and progression
- clear visibility into how much RP is needed to reach Predator rank

## Target user experience

### Public landing page
As a non-authenticated user, I want to see the current Apex status so I can understand the game state at a glance.

Visible elements:
- current map rotation
- current leaderboard snapshot
- server status / health indicators
- selected platform RP thresholds to Predator
- recommended next actions or trending information

### Player search flow
As a public user, I want to search by player name and console so I can see my profile and progression.

The flow:
1. user enters player name and selects platform
2. system validates input and triggers search
3. results page shows summary cards for the player
4. additional sections show performance metrics, weapon stats, map performance, character performance, and rank progression

### Player insight details
As a user, I want to understand my best and most reliable performance patterns so I can improve play.

Display metrics:
- top performing gun by K/D or usage effectiveness
- best map by K/D or win-rate proxy
- best character by K/D or performance trend
- total rank progress and RP to Predator
- recent streak and performance summary

## Product principles
- public-first experience with no required sign-in
- clear operational status information on the home dashboard
- instant visibility into player progression and rank goals
- mobile-friendly, fast-loading dashboard design
- strong observability and resilience behind the scenes
- easy expansion to more analytics and player insights over time

## User stories

### MVP dashboard
- As a public visitor, I can see the current map rotation.
- As a public visitor, I can view the leaderboard status.
- As a public visitor, I can view server health or current status signals.
- As a public visitor, I can see Predator RP thresholds by platform.

### Player lookup
- As a user, I can search for a player name and platform.
- As a user, I can see a summary card for that player.
- As a user, I can view general account metrics.
- As a user, I can understand their RP gap to Predator.

### Insight analytics
- As a user, I can see top gun by K/D.
- As a user, I can see best map by K/D.
- As a user, I can see best character by K/D.
- As a user, I can see the current rank and RP path to the next milestone.

## Release roadmap

### Phase 1: public dashboard foundation
Goal: deliver a polished landing page that feels like a live Apex status hub.

Deliverables:
- hero summary section
- map rotation panel
- leaderboard card(s)
- server status indicators
- RP-to-Predator threshold cards by platform
- responsive dashboard layout

Acceptance criteria:
- page loads without auth
- data is clearly labeled and easy to scan
- cards are responsive and visually polished
- empty/loading/error states are designed

### Phase 2: player lookup and summary
Goal: enable players to search and view their account summary.

Deliverables:
- search input with platform selector
- loading spinner and validation states
- player summary card
- rank card with RP progress
- fallback message for invalid or missing user results

Acceptance criteria:
- user can search by name and platform
- results display quickly and clearly
- invalid input produces a helpful response
- response data is readable and structured

### Phase 3: performance drill-down
Goal: show advanced player performance metrics.

Deliverables:
- top weapon card
- best map card
- best legend/character card
- K/D-based insight comparisons
- rank-to-Predator calculation breakdown

Acceptance criteria:
- insights are derived clearly from data and labeled appropriately
- cards include explanatory context
- comparison values are easy to interpret
- data is consistent across the platform selection

### Phase 4: production quality and scale
Goal: make the product feel credible and operationally mature.

Deliverables:
- retry / timeout handling
- error boundaries and degraded UI states
- OpenTelemetry metrics and tracing
- Grafana dashboards for app and API health
- deployment pipeline toward AWS EKS

Acceptance criteria:
- app degrades gracefully when backend calls fail
- latency and error metrics are visible in dashboards
- deployment process is repeatable and monitored

## Dashboard information architecture

### Public dashboard sections
1. Hero / overview
   - current status summary
   - quick links to search or explore
2. Game state
   - map rotation
   - active status and trending metadata
3. Ranked progression
   - RP to Predator by platform
4. Community snapshot
   - leaderboard summaries
   - server health indicators

### Player profile sections
1. player identity and platform
2. account summary values
3. rank progress and RP target
4. weapon insights
5. map insights
6. legend/character insights
7. recent trend or performance summary

## UX design priorities
- avoid clutter; prioritize the most important stats first
- keep the dashboard useful to both casual players and competitive players
- use strong visual hierarchy for rank and progression metrics
- show empty, loading, and failure states intentionally
- keep the public experience elegant and fast

## Technical implementation approach for UI
The UI should be implemented in phases, starting with the public dashboard and moving into dynamic search and profile analytics.

Recommended UI stack:
- Blazor for the app shell and component structure
- component-based cards for dashboard widgets
- API client abstraction already in place
- service layer for dashboard and player lookups
- tests for rendering, validation, and error handling

## Recommended next step
The next step is UI implementation focused on Phase 1 and Phase 2:
- dashboard landing page
- search experience
- player summary cards
- ranked RP and progression display
- loading/error states

Once the UI is shaped and validated, we will align the backend contract, resilience, and cloud deployment architecture to support it in production.
