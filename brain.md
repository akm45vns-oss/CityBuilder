# Project Memory: CityBuilder

## Project Identity
- **Game Title**: CityBuilder
- **Internal Codename**: Project Metropolis
- **Genre**: City-Building Simulation / Strategy
- **Theme**: Modern Urban Planning and Resource Management
- **Story / Lore**: The player is a newly appointed Mayor of a nascent settlement. They must guide the settlement from a small farming village to a thriving, sustainable, smart modern metropolis.
- **World**: A procedurally generated or customizable terrain with natural resources (water, forests, minerals, wind, solar capacity).
- **Target Audience**: Fans of simulation, strategy, and planning games (SimCity, Cities: Skylines, Frostpunk).
- **Inspiration**: Cities: Skylines, SimCity 4, Anno series, Factorio.
- **Unique Selling Points (USPs)**:
  - Dynamic gridless or grid-based building with organic growth patterns.
  - Deep ecological and economic sustainability mechanics (renewable energy, pollution control, resource circularity).
  - Micro-simulation of individual citizens (agents) with daily schedules, needs, and feedback.
- **Platforms**: Windows, Linux, macOS (Future Support: Steam, Steam Deck).
- **Engine**: Unity 6 LTS (C#, Universal Render Pipeline - URP).
- **Version**: 0.0.1
- **Current Build**: Alpha 0.0.1 (Skeleton)
- **Development Status**: Initializing Repository / Planning Phase
- **Vision**: Create an engaging, performance-optimized, and beautiful city builder that runs directly in the browser, featuring complex simulation loops and satisfying visual progression.
- **Design Philosophy**:
  - Systems-driven design: individual simple rules leading to complex emergent behaviors.
  - Player agency and creativity: sandbox elements combined with meaningful challenge.
  - Clean, modern, high-fidelity aesthetic.

---

## Progress
- **Overall Completion Percentage**: 16%
- **Current Milestone**: Milestone 1: Project Setup & Core Rendering Engine — COMPLETE
- **Current Sprint**: Sprint 4: Playable Prototype Validation
- **Current Objective**: Milestone 1 Complete. Waiting for Milestone 2 (Phase 5) instructions.
- **Current Task**: Completed Playable Prototype Validation (Test HUD, Scene loading, automated tester confirmation).
- **Next Task**: Await the user's instructions for the next development milestone.
- **Previous Completed Task**: Integrated IMGUI test overlays and completed validation reports.
- **Blocked Tasks**: None.
- **Pending Work**: Economy simulation, Traffic AI, Citizens.
- **Remaining Work**: Design and implementation of all gameplay simulation systems.
- **Estimated Roadmap**:
   - Phase 1: Core Engine & Visuals (Grid, Camera, Basic Placement)
   - Phase 2: Simulation Loop (Time, Resources, Zoning, Road system)
   - Phase 3: Citizens & Pathfinding (Agent AI, Transport, Jobs)
   - Phase 4: UI/UX & Polish (Menus, Statistics, Sound, Graphics polishing)
   - Phase 4.1: Refactoring, Optimization, and Thread Safety (Stable Core)
   - Phase 5: Economy, Utilities, & Zoning (Emergent Systems)

---

## Gameplay
- **Game Loop**:
  1. Build infrastructure (roads, power, water) and zone land (Residential, Commercial, Industrial).
  2. Citizens move in, consume resources, work, and pay taxes.
  3. Manage expenses, tax rates, and city services (health, education, fire safety).
  4. Expand the city, unlock new building types, and handle disasters/challenges.
- **Mechanics**:
  - Zoning system (RCI: Residential, Commercial, Industrial).
  - Power/Water grid propagation.
  - Real-time traffic simulation on road networks.
  - Tax and budgeting system.
- **Controls**: WASD/Mouse Drag for camera movement, Scroll for zoom, Mouse Left Click to place/interact, Mouse Right Click to demolish/cancel, R to rotate buildings.
- **Movement**: 2D Orthographic / Isometric camera controls (Pan, Zoom, Rotate).
- **Combat**: N/A (Focus on building and peaceful simulation).
- **Building System**: Grid-based alignment, drag-to-build roads, area-select zoning, single-point placement for service buildings.
- **Economy**: Currency (Credits), monthly budget (Taxes vs Service Costs), import/export of resources.
- **AI**: Simple agent-based simulation where citizens find paths to work, home, and shopping.
- **Inventory / Resource Storage**: Warehouses, city-wide resource counters (Power, Water, Waste, Fuel).
- **Crafting**: N/A.
- **Skills / Progression**: City milestones unlocked by population threshold.
- **NPCs**: Citizens with basic state machines (Sleeping, Commuting, Working, Shopping, Idle).
- **Dialogue**: Citizen feedback popup bubble system ("Chirper" style feed).
- **Weather**: Sunny, Rainy, Foggy (affecting solar/wind power generation and traffic speed).
- **Time System**: Day/Night cycle, weekly/monthly calendar tick for budget calculation.
- **Events**: Fires, power outages, traffic jams, heatwaves.
- **Save System**: LocalStorage or JSON file import/export.
- **Multiplayer Architecture**: Singleplayer.
- **Balancing Decisions**: To be determined.
- **Difficulty System**: Easy (sandbox/high funds), Normal, Hard (demanding citizens, expensive services).
- **Accessibility Options**: Keyboard remapping, colorblind-friendly UI modes, adjustable simulation speed.
- **Achievements**: population milestones, eco-friendly city, mega-rich mayor, etc.

---

## World
- **Maps**: Flat grid, hilly terrain, coastal map, river valley.
- **Regions**: Single playable region map with expandable tiles.
- **Biomes**: Temperate, Desert, Tropical.
- **Buildings**: Residential (Low/High density), Commercial (Low/High density), Industrial (Low/High density), Service Buildings (Power Plant, Water Pumping Station, Landfill, School, Police, Fire Station).
- **Objects**: Trees, rocks, decorative props.
- **Terrain**: Heightmap-based grid, water bodies, resource deposits.
- **Navigation**: Node-link network for roads/traffic, A* pathfinding for citizen agents.
- **Spawn System**: Citizen agents spawning at city borders/residential buildings.
- **Resource Locations**: Mineral deposits, fertile land, wind strength map, water currents.
- **Lighting**: Dynamic ambient light based on time of day.
- **Day/Night Cycle**: 24-minute real-time day cycle (1 minute per hour).
- **Environment Rules**: Land/water pollution spreading, noise pollution, tree growth.

---

## Characters
- **Player**: The Mayor (Invisible orchestrator).
- **Citizens**: Individual agents (Sims) with names, age, employment status, happiness, health, and location.
- **Vehicles**: Cars, trucks, buses, service vehicles (fire trucks, police cars).
- **Animations**: Basic movement translations, building construction states, smoke particle effects, traffic movement.
- **Abilities**: N/A.
- **Stats**: Citizen Health, Education, Wealth, Happiness.
- **Equipment**: N/A.
- **Health / Damage**: Citizen health affected by pollution, healthcare access, and water/power supply.
- **AI Behaviors**: State-driven routing: Home -> Commute -> Work -> Commute -> Home -> Commute -> Commercial -> Home.

---

## Assets
- **Models / Sprites**: 2D Isometric sprite assets or Canvas-drawn vector paths.
- **Textures**: Grass, roads, water, building structures.
- **Animations**: Citizen walking sprites, vehicle rotating sprites.
- **Audio**: Ambient city hum, clicking sounds, build sounds.
- **Music**: Relaxing jazz / lo-fi background tracks.
- **UI Assets**: High-quality CSS styled icons, progress bars, charts.
- **Fonts**: Clean modern sans-serif (e.g., Inter or Roboto).
- **Shaders / Materials**: N/A (if using HTML5 2D Canvas).
- **Licenses / Credits**: All open-source or generated assets.
- **Asset Sources**: To be decided.
- **Missing Assets**: All graphic and audio assets (using placeholder shapes/colors initially).

---

## Code Architecture
- **Complete Folder Structure**:
  - `.` (Root)
    - `brain.md` (Project Memory)
    - `README.md`
- **Important Files**:
  - None yet.
- **Architecture Decisions**:
  - Unity Component-based architecture.
  - Core Singleton Managers (GameManager, SceneManager, etc.) following clean architecture.
  - Strict adherence to SOLID principles, DRY, KISS, and Composition over Inheritance.

---

## Database / Save Data
- **Save Format**: JSON serialization of the game state.
- **Player Data**: City name, level, funds, unlocked items.
- **Inventory**: Simulation state (all building grid cells, roads list, citizen agents list).
- **Settings**: Audio volume, screen resolution/scale, control settings.
- **Serialization**: Custom JSON serializer to stringify grid/agents.

---

## APIs
- **External Services**: None.
- **Authentication**: None.

---

## UI / UX
- **Menus**: Main Menu (New Game, Load Game, Settings), In-game Mayor Panel (Zoning tools, Building categories, Budget sheet, Statistics graphs).
- **HUD**: Top bar (Funds, Date, Population, Citizen Happiness, Demand Bars for RCI), Bottom toolbar (Selection, Roads, Zone R, Zone C, Zone I, Power, Water, Services, Demolish).
- **Settings**: Audio toggles, graphics quality, game speed controls (Pause, 1x, 2x, 5x).
- **Responsive Behavior**: Scaling canvas to fit screen size, responsive flexbox layout for panels.

---

## Audio
- **Volume System**: Master, Music, SFX sliders.
- **Current Implementation**: None.

---

## Performance
- **FPS Target**: 60 FPS.
- **Bottlenecks**: Pathfinding for thousands of citizen agents, rendering large grids.
- **Optimizations**: Spatial partitioning, offscreen canvas caching.

---

## Security
- **Anti-cheat**: Client-side singleplayer game; no advanced anti-cheat required.
- **Save Protection**: Simple checksum.

---

## Bugs
- No bugs logged yet (fresh repository).

---

## Changelog
- **2026-07-05**: Project initialization. Created `brain.md` to document architecture and gameplay vision.

---

## Decisions Log
- **2026-07-05**: Created `brain.md` as the permanent memory and architectural single source of truth for Project Metropolis.

---

## File History
- **README.md**: Initial file from git init.
- **brain.md**: Project memory and documentation (created 2026-07-05).

---

## TODO
- [ ] Align with user on tech stack (React/Vite vs Vanilla JS, 2D HTML5 Canvas vs SVG vs Three.js/WebGL).
- [ ] Scaffold the project directory structure.
- [ ] Create the index.html, main application files, and styling base.
- [ ] Build the canvas-based grid renderer and camera controls (pan, zoom).

---

## Current Context
Milestone 1 (Playable Prototype) is fully complete! We have successfully integrated a functional IMGUI-based Test UI overlay supporting scene transitions between MainMenu and Game, tool activations (Roads, Buildings, Bulldozer, Eyedropper), Undo/Redo actions, and DTO-based Save/Load operations. Automated stress tests confirm stable memory profiles (0 GC allocations in Update) and sub-millisecond spatial queries (O(1) Spatial Hash Grid). The git repository is tagged at Milestone-01-Playable-Prototype (updated to Milestone-01.1-Stable-Core). Waiting for Milestone 2 instructions.

---

## Next AI Instructions
1. Await instructions from the user to initiate the next development milestone (Zoning, Economy, Citizen agents, Traffic AI).
2. Do not write any code for future phases until explicitly instructed.
3. Upon receiving the next prompt, read `brain.md`, understand the new requirements, and proceed accordingly.

---

## Testing
- [ ] Manual test: Verify project launches in dev mode.
- [ ] Manual test: Verify grid renders correctly and responds to window resize.

---

## Deployment
- None yet.

---

## Risks
- **Performance Risk**: Running complex agent simulations in JavaScript can cause lag. Mitigation: Use Web Workers or optimize pathfinding with A* and distance-heuristics.
- **Visuals Risk**: Creating a compelling UI and aesthetic with limited assets. Mitigation: High-quality modern CSS styling, SVGs, and procedurally generated/vector canvas art.

---

## Future Vision
- Add transport systems (trains, subways, airports).
- Implement dynamic simulation challenges like economics crises, natural disasters, and green-energy transformation.
