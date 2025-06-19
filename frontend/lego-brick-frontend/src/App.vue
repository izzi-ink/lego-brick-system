<template>
  <div id="app" class="portal-layout">
    <!-- Top Navigation Bar -->
    <nav class="navbar navbar-expand-lg navbar-dark falcon-navbar fixed-top">
      <div class="container-fluid">
        <a class="navbar-brand d-flex align-items-center" href="#">
          <i class="fas fa-cubes brand-icon me-2"></i>
          <span class="brand-text">LEGO Brick System</span>
        </a>
        
        <div class="navbar-nav ms-auto">
          <div class="nav-item dropdown">
            <a class="nav-link dropdown-toggle d-flex align-items-center" href="#" role="button" data-bs-toggle="dropdown">
              <span class="me-2">Business Analyst Portal</span>
              <div class="avatar-circle">BA</div>
            </a>
          </div>
        </div>
      </div>
    </nav>

    <!-- Main Container -->
    <div class="main-container">
      <!-- Sidebar -->
      <aside class="sidebar">
        <div class="sidebar-content">
          <div class="nav-section">
            <h6 class="nav-section-title">MAIN</h6>
            <ul class="nav nav-pills flex-column">
              <li class="nav-item">
                <a class="nav-link" :class="{ active: activeView === 'dashboard' }" @click="setActiveView('dashboard')" href="#">
                  <i class="fas fa-tachometer-alt icon"></i> Dashboard
                </a>
              </li>
              <li class="nav-item">
                <a class="nav-link" :class="{ active: activeView === 'create' }" @click="setActiveView('create')" href="#">
                  <i class="fas fa-plus icon"></i> Create Brick
                </a>
              </li>
              <li class="nav-item">
                <a class="nav-link" :class="{ active: activeView === 'browse' }" @click="setActiveView('browse')" href="#">
                  <i class="fas fa-search icon"></i> Browse Bricks
                </a>
              </li>
            </ul>
          </div>
          
          <div class="nav-section">
            <h6 class="nav-section-title">ANALYTICS</h6>
            <ul class="nav nav-pills flex-column">
              <li class="nav-item">
                <a class="nav-link" @click="setActiveView('analytics')" href="#">
                  <i class="fas fa-project-diagram icon"></i> Service Map
                </a>
              </li>
              <li class="nav-item">
                <a class="nav-link" @click="setActiveView('reports')" href="#">
                  <i class="fas fa-chart-bar icon"></i> Reports
                </a>
              </li>
            </ul>
          </div>
        </div>
      </aside>

      <!-- Main Content Area -->
      <main class="content-area">
        <!-- Dashboard View -->
        <div v-if="activeView === 'dashboard'" class="view-container">
          <div class="page-header">
            <h1 class="page-title">Dashboard</h1>
            <p class="page-subtitle">Overview of your LEGO Brick System</p>
          </div>
          
          <!-- Stats Cards -->
          <div class="row stats-row">
            <div class="col-lg-3 col-md-6 mb-4">
              <div class="stat-card">
                <div class="stat-card-body">
                  <div class="stat-icon"><i class="fas fa-cubes"></i></div>
                  <div class="stat-content">
                    <h3 class="stat-number">{{ totalBricks }}</h3>
                    <p class="stat-label">Total Bricks</p>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-lg-3 col-md-6 mb-4">
              <div class="stat-card">
                <div class="stat-card-body">
                  <div class="stat-icon"><i class="fas fa-folder-open"></i></div>
                  <div class="stat-content">
                    <h3 class="stat-number">{{ totalProjects }}</h3>
                    <p class="stat-label">Projects</p>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-lg-3 col-md-6 mb-4">
              <div class="stat-card">
                <div class="stat-card-body">
                  <div class="stat-icon"><i class="fas fa-users"></i></div>
                  <div class="stat-content">
                    <h3 class="stat-number">{{ totalCreators }}</h3>
                    <p class="stat-label">Contributors</p>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-lg-3 col-md-6 mb-4">
              <div class="stat-card">
                <div class="stat-card-body">
                  <div class="stat-icon"><i class="fas fa-tags"></i></div>
                  <div class="stat-content">
                    <h3 class="stat-number">{{ totalTags }}</h3>
                    <p class="stat-label">Tags</p>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Quick Actions -->
          <div class="row">
            <div class="col-lg-8">
              <div class="card quick-actions-card">
                <div class="card-header">
                  <h5 class="card-title">Quick Actions</h5>
                </div>
                <div class="card-body">
                  <div class="row">
                    <div class="col-md-6 mb-3">
                      <button class="btn btn-primary btn-lg w-100" @click="setActiveView('create')">
                        <i class="fas fa-plus me-2"></i> Create New Brick
                      </button>
                    </div>
                    <div class="col-md-6 mb-3">
                      <button class="btn btn-outline-primary btn-lg w-100" @click="setActiveView('browse')">
                        <i class="fas fa-search me-2"></i> Browse All Bricks
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-lg-4">
              <div class="card recent-activity-card">
                <div class="card-header">
                  <h5 class="card-title">Recent Activity</h5>
                </div>
                <div class="card-body">
                  <p class="text-muted">Recent bricks and updates will appear here</p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Create Brick View -->
        <div v-if="activeView === 'create'" class="view-container">
          <div class="page-header">
            <h1 class="page-title">Create New Brick</h1>
            <p class="page-subtitle">Add a new service or feature to the system</p>
          </div>
          <BrickForm @brick-created="handleBrickCreated" />
        </div>

        <!-- Browse Bricks View -->
        <div v-if="activeView === 'browse'" class="view-container">
          <div class="page-header">
            <h1 class="page-title">Browse Bricks</h1>
            <p class="page-subtitle">Explore and manage all system components</p>
          </div>
          <BrickList ref="brickList" @brick-updated="updateBricksData" @brick-deleted="updateBricksData" />
        </div>

        <!-- Analytics/Service Map View -->
        <div v-if="activeView === 'analytics'" class="view-container">
          <div class="page-header">
            <h1 class="page-title">Service Map</h1>
            <p class="page-subtitle">Visualize service relationships and data flow</p>
          </div>
          <div class="card">
            <div class="card-body text-center py-5">
              <h5 class="text-muted">Service Map Visualization</h5>
              <p class="text-muted">Coming soon - Interactive service dependency mapping</p>
            </div>
          </div>
        </div>

        <!-- Reports View -->
        <div v-if="activeView === 'reports'" class="view-container">
          <div class="page-header">
            <h1 class="page-title">Reports</h1>
            <p class="page-subtitle">Generate insights and documentation</p>
          </div>
          <div class="card">
            <div class="card-body text-center py-5">
              <h5 class="text-muted">Reports & Analytics</h5>
              <p class="text-muted">Coming soon - Detailed reporting and export functionality</p>
            </div>
          </div>
        </div>
      </main>
    </div>
  </div>
</template>

<script>
import BrickForm from './components/BrickForm.vue'
import BrickList from './components/BrickList.vue'

export default {
  name: 'App',
  components: {
    BrickForm,
    BrickList
  },
  data() {
    return {
      activeView: 'dashboard',
      bricks: []
    }
  },
  computed: {
    totalBricks() {
      return this.bricks.length
    },
    totalProjects() {
      const projects = new Set(this.bricks.map(brick => brick.projectName).filter(p => p))
      return projects.size
    },
    totalCreators() {
      const creators = new Set(this.bricks.map(brick => brick.creatorName).filter(c => c))
      return creators.size
    },
    totalTags() {
      const allTags = []
      this.bricks.forEach(brick => {
        if (brick.tags) {
          allTags.push(...brick.tags.split(',').map(tag => tag.trim()).filter(tag => tag))
        }
      })
      const uniqueTags = new Set(allTags)
      return uniqueTags.size
    }
  },
  methods: {
    setActiveView(view) {
      this.activeView = view
    },
    handleBrickCreated() {
      this.refreshBrickList()
      this.setActiveView('dashboard')
    },
    refreshBrickList() {
      if (this.$refs.brickList) {
        this.$refs.brickList.fetchBricks()
      }
      this.updateBricksData()
    },
    async updateBricksData() {
      try {
        const response = await fetch('http://localhost:5213/api/bricks')
        if (response.ok) {
          this.bricks = await response.json()
        }
      } catch (error) {
        console.error('Error fetching bricks for stats:', error)
      }
    }
  },
  mounted() {
    this.updateBricksData()
  }
}
</script>

<style>
/* Custom Green Palette Variables */
:root {
  /* Base Palette */
  --feldgrau: #3c493f;
  --battleship-gray: #7e8d85;
  --ash-gray: #b3bfb8;
  --mint-cream: #f0f7f4;
  --celadon: #394d43;
  
  /* Theme Mapping */
  --primary-color: var(--celadon);
  --primary-dark: #2d3a31;
  --sidebar-bg: var(--feldgrau);
  --sidebar-border: var(--battleship-gray);
  --sidebar-text: #ffffff;
  --sidebar-text-muted: var(--ash-gray);
  --sidebar-hover: #79a38f;
  --sidebar-active: rgba(57, 77, 67, 0.15);
  --content-bg: var(--mint-cream);
  --card-bg: #ffffff;
  --text-primary: var(--feldgrau);
  --text-secondary: var(--battleship-gray);
  --border-color: var(--ash-gray);
  --accent-light: rgba(57, 77, 67, 0.1);
  
  /* Override Bootstrap primary color */
  --bs-primary: #696D7D;
  --bs-primary-rgb: 105, 109, 125;
  
  /* Override Bootstrap navigation pills */
  --bs-nav-pills-link-active-bg: #28a745;
  --bs-nav-pills-link-active-color: #ffffff;
  --bs-nav-link-hover-color: #28a745;
}

/* Global Portal Styles */
* {
  box-sizing: border-box;
}

body {
  background-color: var(--content-bg);
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', sans-serif;
  margin: 0;
  padding: 0;
}

.portal-layout {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

/* Top Navigation */
.falcon-navbar {
  background-color: var(--primary-color) !important;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  z-index: 1030;
}

.brand-icon {
  font-size: 1.5rem;
}

.brand-text {
  font-weight: 600;
  font-size: 1.25rem;
}

.avatar-circle {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  background-color: rgba(255,255,255,0.2);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.8rem;
  font-weight: 600;
  color: white;
}

/* Main Layout */
.main-container {
  display: flex;
  flex: 1;
  margin-top: 56px; /* navbar height */
}

/* Sidebar */
.sidebar {
  width: 280px;
  background: var(--sidebar-bg);
  border-right: 1px solid var(--sidebar-border);
  box-shadow: 2px 0 4px rgba(0,0,0,0.15);
  position: fixed;
  top: 56px;
  left: 0;
  height: calc(100vh - 56px);
  overflow-y: auto;
  z-index: 1020;
}

.sidebar-content {
  padding: 1.5rem 0;
}

.nav-section {
  margin-bottom: 2rem;
}

.nav-section-title {
  font-size: 0.75rem;
  font-weight: 700;
  color: var(--sidebar-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.5px;
  padding: 0 1.5rem;
  margin-bottom: 0.75rem;
}

.nav-pills .nav-link {
  color: var(--sidebar-text);
  border-radius: 0;
  padding: 0.75rem 1.5rem;
  margin: 0;
  border: none;
  display: flex;
  align-items: center;
  font-weight: 500;
  transition: all 0.2s ease;
}

.nav-pills .nav-link:hover {
  background-color: var(--sidebar-hover);
  color: var(--primary-color);
}

.nav-pills .nav-link.active {
  background-color: #28a745 !important;
  color: #ffffff !important;
  border-right: 3px solid #28a745 !important;
}

.nav-pills .nav-link.active:hover {
  background-color: #20c997 !important;
  color: #ffffff !important;
}

/* Force override Bootstrap nav pills */
.nav-pills .nav-item .nav-link.active {
  background-color: #28a745 !important;
  color: #ffffff !important;
}

.sidebar .nav-pills .nav-link.active {
  background-color: #28a745 !important;
  color: #ffffff !important;
  border-right: 3px solid #28a745 !important;
}

/* Ultra-specific override for active nav items */
a.nav-link.active,
a.nav-link.active:hover,
a.nav-link.active:focus {
  background-color: #28a745 !important;
  color: #ffffff !important;
  border-right: 3px solid #28a745 !important;
}

/* Target the exact element structure */
.sidebar .nav-pills .nav-item .nav-link.active {
  background-color: #28a745 !important;
  color: #ffffff !important;
  border-right: 3px solid #28a745 !important;
}

.nav-pills .nav-link .icon {
  margin-right: 0.75rem;
  font-size: 1rem;
  width: 16px;
  text-align: center;
}

/* Content Area */
.content-area {
  flex: 1;
  margin-left: 280px;
  padding: 0;
  background-color: var(--content-bg);
  min-height: calc(100vh - 56px);
}

.view-container {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

/* Page Header */
.page-header {
  margin-bottom: 2rem;
}

.page-title {
  font-size: 2rem;
  font-weight: 700;
  color: var(--text-primary);
  margin-bottom: 0.5rem;
}

.page-subtitle {
  font-size: 1.1rem;
  color: var(--text-secondary);
  margin-bottom: 0;
}

/* Stats Cards */
.stats-row {
  margin-bottom: 2rem;
}

.stat-card {
  background: var(--card-bg);
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(60, 73, 63, 0.08);
  border: 1px solid var(--border-color);
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  height: 100%;
}

.stat-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(57, 77, 67, 0.2);
  border-color: var(--primary-color);
}

.stat-card-body {
  padding: 1.5rem;
  display: flex;
  align-items: center;
}

.stat-icon {
  font-size: 2.5rem;
  margin-right: 1rem;
  opacity: 0.8;
}

.stat-content {
  flex: 1;
}

.stat-number {
  font-size: 2rem;
  font-weight: 700;
  color: var(--text-primary);
  margin: 0;
  line-height: 1;
}

.stat-label {
  font-size: 0.9rem;
  color: var(--text-secondary);
  margin: 0.25rem 0 0 0;
  font-weight: 500;
}

/* Cards */
.card {
  border: 1px solid var(--border-color);
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(60, 73, 63, 0.08);
  background: var(--card-bg);
}

.card-header {
  background-color: var(--content-bg);
  border-bottom: 1px solid var(--border-color);
  padding: 1rem 1.5rem;
  border-radius: 12px 12px 0 0 !important;
}

.card-title {
  margin: 0;
  font-weight: 600;
  color: var(--text-primary);
}

.card-body {
  padding: 1.5rem;
}

/* Quick Actions */
.quick-actions-card .btn {
  border-radius: 8px;
  font-weight: 500;
  padding: 0.75rem 1rem;
  transition: all 0.2s ease;
}

.btn-primary {
  background-color: var(--primary-color);
  border-color: var(--primary-color);
  color: #ffffff;
}

.btn-primary:hover {
  background-color: var(--primary-dark);
  border-color: var(--primary-dark);
  color: #ffffff;
}

.btn-outline-primary {
  color: var(--primary-color);
  border-color: var(--primary-color);
}

.btn-outline-primary:hover {
  background-color: var(--primary-color);
  border-color: var(--primary-color);
  color: #ffffff;
}

/* Override any remaining Bootstrap blue colors */
.btn-outline-secondary {
  color: #696D7D;
  border-color: #696D7D;
}

.btn-outline-secondary:hover {
  background-color: #696D7D;
  border-color: #696D7D;
  color: #ffffff;
}

/* Comprehensive Bootstrap blue overrides */
.btn-check:checked + .btn-outline-primary,
.btn-outline-primary.active,
.btn-outline-primary:active {
  background-color: var(--primary-color) !important;
  border-color: var(--primary-color) !important;
  color: #ffffff !important;
}

.btn-outline-primary:focus {
  box-shadow: 0 0 0 0.2rem rgba(57, 77, 67, 0.25) !important;
}

/* Override view toggle buttons */
.view-options .btn-outline-primary {
  color: #696D7D !important;
  border-color: #696D7D !important;
}

.view-options .btn-outline-primary:hover {
  background-color: #696D7D !important;
  border-color: #696D7D !important;
  color: #ffffff !important;
}

.view-options .btn-check:checked + .btn-outline-primary {
  background-color: #696D7D !important;
  border-color: #696D7D !important;
  color: #ffffff !important;
}

/* Global Bootstrap primary color overrides */
.btn-primary,
.bg-primary,
.text-primary,
.border-primary {
  background-color: #696D7D !important;
  border-color: #696D7D !important;
  color: #ffffff !important;
}

.btn-primary:hover,
.btn-primary:focus,
.btn-primary:active {
  background-color: #5a5e6a !important;
  border-color: #5a5e6a !important;
  color: #ffffff !important;
}

.text-primary {
  color: #696D7D !important;
}

.border-primary {
  border-color: #696D7D !important;
}

/* Additional Bootstrap utility overrides */
.link-primary {
  color: #696D7D !important;
}

.link-primary:hover {
  color: #5a5e6a !important;
}

.quick-actions-card .btn:hover {
  transform: translateY(-1px);
}

/* Responsive Design */
@media (max-width: 1199.98px) {
  .sidebar {
    width: 250px;
  }
  
  .content-area {
    margin-left: 250px;
  }
}

@media (max-width: 991.98px) {
  .sidebar {
    transform: translateX(-100%);
    transition: transform 0.3s ease;
  }
  
  .content-area {
    margin-left: 0;
  }
  
  .view-container {
    padding: 1rem;
  }
  
  .page-title {
    font-size: 1.75rem;
  }
  
  .stat-card-body {
    padding: 1rem;
  }
  
  .stat-icon {
    font-size: 2rem;
  }
  
  .stat-number {
    font-size: 1.5rem;
  }
}

@media (max-width: 767.98px) {
  .view-container {
    padding: 0.75rem;
  }
  
  .page-header {
    margin-bottom: 1.5rem;
  }
  
  .page-title {
    font-size: 1.5rem;
  }
  
  .page-subtitle {
    font-size: 1rem;
  }
  
  .stats-row {
    margin-bottom: 1.5rem;
  }
  
  .stat-card-body {
    flex-direction: column;
    text-align: center;
    padding: 1rem;
  }
  
  .stat-icon {
    margin-right: 0;
    margin-bottom: 0.5rem;
  }
}

/* Custom Form Styling for Portal */
.form-control, .form-select {
  border-radius: 8px;
  border: 1px solid #ced4da;
  transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
}

.form-control:focus, .form-select:focus {
  border-color: var(--primary-color);
  outline: 0;
  box-shadow: 0 0 0 0.2rem rgba(57, 77, 67, 0.25);
}

.btn {
  border-radius: 8px;
  font-weight: 500;
  transition: all 0.2s ease;
}

.btn:hover {
  transform: translateY(-1px);
}

.btn:focus {
  box-shadow: 0 0 0 0.2rem rgba(57, 77, 67, 0.25);
}

/* Animation for smooth transitions */
.view-container {
  animation: fadeIn 0.3s ease-in;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>