# CLAUDE.md - Development Guide

## Build & Run Commands

### Frontend (Vue.js)
- Build: `cd frontend/lego-brick-frontend && npm run build`
- Dev server: `cd frontend/lego-brick-frontend && npm run serve`
- Lint: `cd frontend/lego-brick-frontend && npm run lint`

### Backend (.NET)
- Build: `cd backend/LegoBrickApi && dotnet build`
- Run API: `cd backend/LegoBrickApi && dotnet run`
- Watch mode: `cd backend/LegoBrickApi && dotnet watch run`

## Code Style Guidelines

### Frontend
- Follow Vue 3 Composition API patterns
- Use camelCase for variables/methods, PascalCase for components
- Consistent imports: Vue components first, then utilities
- Handle API errors with try/catch blocks

### Backend
- C# naming: PascalCase for classes/methods, camelCase for parameters
- Use async/await for DB operations
- Organize controllers with RESTful endpoint patterns
- Include validation for request models