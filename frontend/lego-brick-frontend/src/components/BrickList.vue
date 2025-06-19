<template>
  <div class="list-container">
    
    <!-- Search and Filter Section -->
    <div class="filters-card">
      <div class="card">
        <div class="card-body">
          <div class="row g-3">
            <div class="col-lg-4 col-md-6">
              <div class="filter-group">
                <label for="searchText" class="form-label">
                  <i class="fas fa-search search-icon"></i> Search
                </label>
                <input 
                  type="text" 
                  id="searchText"
                  class="form-control search-input" 
                  v-model="searchText"
                  placeholder="Search services, projects, descriptions..."
                >
              </div>
            </div>
            <div class="col-lg-3 col-md-6">
              <div class="filter-group">
                <label for="projectFilter" class="form-label">
                  <i class="fas fa-folder-open filter-icon"></i> Project
                </label>
                <select id="projectFilter" class="form-select" v-model="selectedProject">
                  <option value="">All Projects</option>
                  <option v-for="project in uniqueProjects" :key="project" :value="project">
                    {{ project }}
                  </option>
                </select>
              </div>
            </div>
            <div class="col-lg-3 col-md-6">
              <div class="filter-group">
                <label for="tagFilter" class="form-label">
                  <i class="fas fa-tags filter-icon"></i> Tag
                </label>
                <select id="tagFilter" class="form-select" v-model="selectedTag">
                  <option value="">All Tags</option>
                  <option v-for="tag in uniqueTags" :key="tag" :value="tag">
                    {{ tag }}
                  </option>
                </select>
              </div>
            </div>
            <div class="col-lg-2 col-md-6">
              <div class="filter-group">
                <label class="form-label">&nbsp;</label>
                <button class="btn btn-outline-secondary w-100 clear-btn" @click="clearFilters">
                  <i class="fas fa-times clear-icon"></i> Clear
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Results Summary -->
    <div class="results-summary" v-if="!loading">
      <div class="d-flex justify-content-between align-items-center">
        <div class="results-info">
          <span class="results-badge">
            {{ filteredBricks.length }} of {{ bricks.length }} bricks
          </span>
        </div>
        <div class="view-options">
          <div class="btn-group" role="group">
            <input type="radio" class="btn-check" name="viewType" id="grid-view" autocomplete="off" v-model="viewType" value="grid" checked>
            <label class="btn btn-outline-primary btn-sm" for="grid-view"><i class="fas fa-th"></i> Grid</label>
            
            <input type="radio" class="btn-check" name="viewType" id="list-view" autocomplete="off" v-model="viewType" value="list">
            <label class="btn btn-outline-primary btn-sm" for="list-view"><i class="fas fa-list"></i> List</label>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Loading message -->
    <div v-if="loading" class="alert alert-info">
      Loading bricks...
    </div>
    
    <!-- Error message -->
    <div v-if="error" class="alert alert-danger">
      Error: {{ error }}
    </div>
    
    <!-- No bricks message -->
    <div v-if="!loading && bricks.length === 0" class="alert alert-warning">
      No bricks found. Create your first brick!
    </div>
    
    <!-- No filtered results -->
    <div v-if="!loading && bricks.length > 0 && filteredBricks.length === 0" class="alert alert-info">
      No bricks match your search criteria. Try adjusting your filters.
    </div>
    
    <!-- Bricks display -->
    <div v-if="!loading" class="bricks-container">
      <!-- Grid View -->
      <div v-if="viewType === 'grid'" class="row g-4">
        <div v-for="brick in filteredBricks" :key="`brick-${brick.id}`" class="col-xl-4 col-lg-6 col-md-6">
          <div class="brick-card">
            <div class="brick-card-header">
              <div class="service-info">
                <h5 class="service-name" v-if="!isEditing(brick.id)">{{ brick.serviceName }}</h5>
                <input 
                  v-else-if="editData[brick.id]"
                  type="text" 
                  class="form-control service-name-edit" 
                  v-model="editData[brick.id].serviceName"
                  @keyup.enter="saveBrick(brick.id)"
                  @keyup.escape="cancelEdit(brick.id)"
                >
                <span class="project-badge" v-if="!isEditing(brick.id)">{{ brick.projectName }}</span>
                <input 
                  v-else-if="editData[brick.id]"
                  type="text" 
                  class="form-control project-name-edit" 
                  v-model="editData[brick.id].projectName"
                  @keyup.enter="saveBrick(brick.id)"
                  @keyup.escape="cancelEdit(brick.id)"
                >
              </div>
              <div class="brick-actions">
                <div v-if="!isEditing(brick.id)" class="action-buttons">
                  <button class="btn btn-sm btn-outline-primary edit-btn" @click="startEdit(brick)" title="Edit brick">
                    <i class="fas fa-edit"></i>
                  </button>
                  <button class="btn btn-sm btn-outline-danger delete-btn" @click="deleteBrick(brick.id)" title="Delete brick">
                    <i class="fas fa-trash"></i>
                  </button>
                </div>
                <div v-else class="edit-actions">
                  <button class="btn btn-sm btn-success save-btn" @click="saveBrick(brick.id)" title="Save changes">
                    <i class="fas fa-check"></i>
                  </button>
                  <button class="btn btn-sm btn-secondary cancel-btn" @click="cancelEdit(brick.id)" title="Cancel editing">
                    <i class="fas fa-times"></i>
                  </button>
                </div>
                <div class="brick-icon"><i class="fas fa-cube"></i></div>
              </div>
            </div>
            <div class="brick-card-body">
              <div class="description-section">
                <p class="service-description" v-if="!isEditing(brick.id)">{{ brick.description }}</p>
                <textarea 
                  v-else-if="editData[brick.id]"
                  class="form-control description-edit" 
                  v-model="editData[brick.id].description"
                  rows="3"
                  @keyup.escape="cancelEdit(brick.id)"
                  placeholder="Brief description of what this service does"
                ></textarea>
              </div>
              
              <div class="data-flow">
                <div class="data-item">
                  <div class="data-label">
                    <i class="fas fa-arrow-down data-icon"></i> Consumes
                  </div>
                  <div class="data-value" v-if="!isEditing(brick.id)">{{ brick.dataConsumed || 'None specified' }}</div>
                  <input 
                    v-else-if="editData[brick.id]"
                    type="text" 
                    class="form-control data-edit" 
                    v-model="editData[brick.id].dataConsumed"
                    @keyup.enter="saveBrick(brick.id)"
                    @keyup.escape="cancelEdit(brick.id)"
                    placeholder="e.g., username, password, session tokens"
                  >
                </div>
                
                <div class="data-item">
                  <div class="data-label">
                    <i class="fas fa-arrow-up data-icon"></i> Produces
                  </div>
                  <div class="data-value" v-if="!isEditing(brick.id)">{{ brick.dataProduced || 'None specified' }}</div>
                  <input 
                    v-else-if="editData[brick.id]"
                    type="text" 
                    class="form-control data-edit" 
                    v-model="editData[brick.id].dataProduced"
                    @keyup.enter="saveBrick(brick.id)"
                    @keyup.escape="cancelEdit(brick.id)"
                    placeholder="e.g., authentication status, user profile"
                  >
                </div>
              </div>
              
              <div class="tags-section">
                <div v-if="!isEditing(brick.id)" class="tags-display">
                  <span 
                    v-for="tag in getTags(brick.tags)" 
                    :key="`${brick.id}-${tag}`" 
                    class="tag-chip"
                  >
                    {{ tag }}
                  </span>
                </div>
                <div v-else-if="editData[brick.id]" class="tags-edit">
                  <label class="form-label-small">Tags</label>
                  <input 
                    type="text" 
                    class="form-control tags-input" 
                    v-model="editData[brick.id].tags"
                    @keyup.enter="saveBrick(brick.id)"
                    @keyup.escape="cancelEdit(brick.id)"
                    placeholder="e.g., auth, security, user-management (comma-separated)"
                  >
                </div>
              </div>
            </div>
            <div class="brick-card-footer">
              <div class="creator-info">
                <div class="creator-avatar">{{ getInitials(isEditing(brick.id) && editData[brick.id] ? editData[brick.id].creatorName : brick.creatorName) }}</div>
                <div class="creator-details">
                  <span class="creator-name" v-if="!isEditing(brick.id)">{{ brick.creatorName }}</span>
                  <input 
                    v-else-if="editData[brick.id]"
                    type="text" 
                    class="form-control creator-name-edit" 
                    v-model="editData[brick.id].creatorName"
                    @keyup.enter="saveBrick(brick.id)"
                    @keyup.escape="cancelEdit(brick.id)"
                    placeholder="Your name"
                  >
                  <span class="creation-date">{{ formatDate(brick.dateCreated) }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- List View -->
      <div v-if="viewType === 'list'" class="list-view">
        <div class="table-responsive">
          <table class="table table-hover">
            <thead>
              <tr>
                <th>Service</th>
                <th>Project</th>
                <th>Description</th>
                <th>Data Flow</th>
                <th>Tags</th>
                <th>Creator</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="brick in filteredBricks" :key="`list-brick-${brick.id}`" class="brick-row">
                <td>
                  <div class="service-cell">
                    <strong v-if="!isEditing(brick.id)">{{ brick.serviceName }}</strong>
                    <input 
                      v-else-if="editData[brick.id]"
                      type="text" 
                      class="form-control form-control-sm" 
                      v-model="editData[brick.id].serviceName"
                      @keyup.enter="saveBrick(brick.id)"
                      @keyup.escape="cancelEdit(brick.id)"
                    >
                  </div>
                </td>
                <td>
                  <span v-if="!isEditing(brick.id)" class="project-badge-small">{{ brick.projectName }}</span>
                  <input 
                    v-else-if="editData[brick.id]"
                    type="text" 
                    class="form-control form-control-sm" 
                    v-model="editData[brick.id].projectName"
                    @keyup.enter="saveBrick(brick.id)"
                    @keyup.escape="cancelEdit(brick.id)"
                  >
                </td>
                <td>
                  <div class="description-cell" v-if="!isEditing(brick.id)">{{ brick.description }}</div>
                  <textarea 
                    v-else-if="editData[brick.id]"
                    class="form-control form-control-sm" 
                    v-model="editData[brick.id].description"
                    rows="2"
                    @keyup.escape="cancelEdit(brick.id)"
                  ></textarea>
                </td>
                <td>
                  <div class="data-flow-cell" v-if="!isEditing(brick.id)">
                    <div v-if="brick.dataConsumed" class="data-mini">
                      <i class="fas fa-arrow-down"></i> {{ brick.dataConsumed }}
                    </div>
                    <div v-if="brick.dataProduced" class="data-mini">
                      <i class="fas fa-arrow-up"></i> {{ brick.dataProduced }}
                    </div>
                  </div>
                  <div v-else-if="editData[brick.id]" class="data-edit-cell">
                    <input 
                      type="text" 
                      class="form-control form-control-sm mb-1" 
                      v-model="editData[brick.id].dataConsumed"
                      placeholder="Data consumed"
                      @keyup.enter="saveBrick(brick.id)"
                      @keyup.escape="cancelEdit(brick.id)"
                    >
                    <input 
                      type="text" 
                      class="form-control form-control-sm" 
                      v-model="editData[brick.id].dataProduced"
                      placeholder="Data produced"
                      @keyup.enter="saveBrick(brick.id)"
                      @keyup.escape="cancelEdit(brick.id)"
                    >
                  </div>
                </td>
                <td>
                  <div class="tags-cell" v-if="!isEditing(brick.id)">
                    <span 
                      v-for="tag in getTags(brick.tags).slice(0, 2)" 
                      :key="`list-${brick.id}-${tag}`" 
                      class="tag-chip-small"
                    >
                      {{ tag }}
                    </span>
                    <span v-if="getTags(brick.tags).length > 2" class="more-tags">
                      +{{ getTags(brick.tags).length - 2 }}
                    </span>
                  </div>
                  <input 
                    v-else-if="editData[brick.id]"
                    type="text" 
                    class="form-control form-control-sm" 
                    v-model="editData[brick.id].tags"
                    placeholder="Tags (comma-separated)"
                    @keyup.enter="saveBrick(brick.id)"
                    @keyup.escape="cancelEdit(brick.id)"
                  >
                </td>
                <td>
                  <div class="creator-cell" v-if="!isEditing(brick.id)">
                    <div class="creator-avatar-small">{{ getInitials(brick.creatorName) }}</div>
                    <div class="creator-info-small">
                      <div class="creator-name-small">{{ brick.creatorName }}</div>
                      <div class="creation-date-small">{{ formatDate(brick.dateCreated) }}</div>
                    </div>
                    <div class="list-actions">
                      <button class="btn btn-sm btn-outline-primary" @click="startEdit(brick)" title="Edit">
                        <i class="fas fa-edit"></i>
                      </button>
                      <button class="btn btn-sm btn-outline-danger" @click="deleteBrick(brick.id)" title="Delete">
                        <i class="fas fa-trash"></i>
                      </button>
                    </div>
                  </div>
                  <div v-else-if="editData[brick.id]" class="creator-edit-cell">
                    <input 
                      type="text" 
                      class="form-control form-control-sm mb-1" 
                      v-model="editData[brick.id].creatorName"
                      placeholder="Creator name"
                      @keyup.enter="saveBrick(brick.id)"
                      @keyup.escape="cancelEdit(brick.id)"
                    >
                    <div class="list-edit-actions">
                      <button class="btn btn-sm btn-success" @click="saveBrick(brick.id)" title="Save">
                        <i class="fas fa-check"></i>
                      </button>
                      <button class="btn btn-sm btn-secondary" @click="cancelEdit(brick.id)" title="Cancel">
                        <i class="fas fa-times"></i>
                      </button>
                    </div>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'BrickList',
  data() {
    return {
      bricks: [],
      loading: true,
      error: null,
      searchText: '',
      selectedProject: '',
      selectedTag: '',
      viewType: 'grid',
      editingBricks: new Set(),
      editData: {},
      savingBricks: new Set()
    }
  },
  computed: {
    filteredBricks() {
      if (!this.bricks || this.bricks.length === 0) return []
      
      let filtered = [...this.bricks]
      
      // Filter by search text
      if (this.searchText && this.searchText.trim()) {
        const searchLower = this.searchText.toLowerCase().trim()
        filtered = filtered.filter(brick => 
          (brick.serviceName && brick.serviceName.toLowerCase().includes(searchLower)) ||
          (brick.projectName && brick.projectName.toLowerCase().includes(searchLower)) ||
          (brick.description && brick.description.toLowerCase().includes(searchLower)) ||
          (brick.dataConsumed && brick.dataConsumed.toLowerCase().includes(searchLower)) ||
          (brick.dataProduced && brick.dataProduced.toLowerCase().includes(searchLower)) ||
          (brick.creatorName && brick.creatorName.toLowerCase().includes(searchLower))
        )
      }
      
      // Filter by project
      if (this.selectedProject) {
        filtered = filtered.filter(brick => brick.projectName === this.selectedProject)
      }
      
      // Filter by tag
      if (this.selectedTag) {
        filtered = filtered.filter(brick => {
          const tags = this.getTags(brick.tags)
          return tags.includes(this.selectedTag)
        })
      }
      
      return filtered
    },
    
    uniqueProjects() {
      if (!this.bricks || this.bricks.length === 0) return []
      const projects = [...new Set(this.bricks.map(brick => brick.projectName).filter(p => p))]
      return projects.sort()
    },
    
    uniqueTags() {
      if (!this.bricks || this.bricks.length === 0) return []
      const allTags = []
      this.bricks.forEach(brick => {
        allTags.push(...this.getTags(brick.tags))
      })
      const uniqueTags = [...new Set(allTags.filter(tag => tag))]
      return uniqueTags.sort()
    }
  },
  methods: {
    async fetchBricks() {
      try {
        this.loading = true
        this.error = null
        
        // Update this port to match your API
        const response = await axios.get('http://localhost:5213/api/bricks')
        this.bricks = response.data || []
        
      } catch (error) {
        console.error('Error fetching bricks:', error)
        this.error = 'Failed to load bricks. Make sure your API is running!'
      } finally {
        this.loading = false
      }
    },
    
    getTags(tagsString) {
      if (!tagsString) return []
      return tagsString.split(',').map(tag => tag.trim()).filter(tag => tag.length > 0)
    },
    
    formatDate(dateString) {
      if (!dateString) return 'Unknown'
      return new Date(dateString).toLocaleDateString()
    },
    
    clearFilters() {
      this.searchText = ''
      this.selectedProject = ''
      this.selectedTag = ''
    },
    
    getInitials(name) {
      if (!name) return '?'
      return name.split(' ').map(word => word.charAt(0).toUpperCase()).slice(0, 2).join('')
    },
    
    isEditing(brickId) {
      return this.editingBricks.has(brickId)
    },
    
    
    startEdit(brick) {
      this.editingBricks.add(brick.id)
      this.editData[brick.id] = {
        serviceName: brick.serviceName,
        projectName: brick.projectName,
        description: brick.description,
        dataConsumed: brick.dataConsumed || '',
        dataProduced: brick.dataProduced || '',
        tags: brick.tags || '',
        creatorName: brick.creatorName
      }
    },
    
    cancelEdit(brickId) {
      this.editingBricks.delete(brickId)
      delete this.editData[brickId]
    },
    
    async saveBrick(brickId) {
      if (this.savingBricks.has(brickId)) return
      
      try {
        this.savingBricks.add(brickId)
        
        const editedData = this.editData[brickId]
        if (!editedData.serviceName || !editedData.projectName || !editedData.description || !editedData.creatorName) {
          alert('Please fill in all required fields (Service Name, Project Name, Description, and Creator Name)')
          return
        }
        
        const updateData = {
          id: brickId,
          ...editedData
        }
        
        await axios.put(`http://localhost:5213/api/bricks/${brickId}`, updateData)
        
        const brickIndex = this.bricks.findIndex(b => b.id === brickId)
        if (brickIndex !== -1) {
          this.bricks[brickIndex] = { ...this.bricks[brickIndex], ...editedData }
        }
        
        this.editingBricks.delete(brickId)
        delete this.editData[brickId]
        
        this.$emit('brick-updated')
        
      } catch (error) {
        console.error('Error updating brick:', error)
        alert('Failed to update brick. Please try again.')
      } finally {
        this.savingBricks.delete(brickId)
      }
    },
    
    async deleteBrick(brickId) {
      if (!confirm('Are you sure you want to delete this brick?')) {
        return
      }
      
      try {
        await axios.delete(`http://localhost:5213/api/bricks/${brickId}`)
        
        this.bricks = this.bricks.filter(brick => brick.id !== brickId)
        
        if (this.isEditing(brickId)) {
          this.cancelEdit(brickId)
        }
        
        this.$emit('brick-deleted')
        
      } catch (error) {
        console.error('Error deleting brick:', error)
        alert('Failed to delete brick. Please try again.')
      }
    }
  },
  
  created() {
    this.fetchBricks()
  }
}
</script>

<style scoped>
.list-container {
  max-width: 100%;
}

/* Filters Card */
.filters-card {
  margin-bottom: 2rem;
}

.filters-card .card {
  border: 1px solid #e9ecef;
  border-radius: 16px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.06);
}

.filter-group {
  height: 100%;
}

.form-label {
  font-weight: 600;
  color: #495057;
  margin-bottom: 0.5rem;
  display: flex;
  align-items: center;
}

.search-icon, .filter-icon, .clear-icon {
  margin-right: 0.5rem;
  font-size: 0.9rem;
}

.search-input {
  border-radius: 10px;
  border: 1px solid #ced4da;
  padding: 0.75rem 1rem;
  transition: all 0.2s ease;
}

.search-input:focus {
  border-color: #394d43;
  box-shadow: 0 0 0 0.2rem rgba(57, 77, 67, 0.25);
  transform: scale(1.01);
}

.clear-btn {
  border-radius: 10px;
  font-weight: 500;
  transition: all 0.2s ease;
}

.clear-btn:hover {
  transform: translateY(-1px);
}

/* Results Summary */
.results-summary {
  margin-bottom: 1.5rem;
  padding: 1rem 0;
}

.results-badge {
  background: linear-gradient(135deg, #f0f7f4 0%, #e6f3ea 100%);
  color: #3c493f;
  padding: 0.5rem 1rem;
  border-radius: 20px;
  font-weight: 600;
  font-size: 0.9rem;
  border: 1px solid #394d43;
}

.view-options .btn-group .btn {
  border-radius: 8px;
  font-size: 0.875rem;
  padding: 0.5rem 1rem;
  font-weight: 500;
}

.view-options .btn-group .btn:first-child {
  border-radius: 8px 0 0 8px;
}

.view-options .btn-group .btn:last-child {
  border-radius: 0 8px 8px 0;
}

/* Grid View - Brick Cards */
.brick-card {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.08);
  border: 1px solid #e9ecef;
  transition: all 0.3s ease;
  height: 100%;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.brick-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 24px rgba(57, 77, 67, 0.2);
  border-color: #394d43;
}

.brick-card-header {
  padding: 1.5rem 1.5rem 1rem 1.5rem;
  display: flex;
  justify-content: between;
  align-items: flex-start;
  border-bottom: 1px solid #f8f9fa;
}

.service-info {
  flex: 1;
}

.service-name {
  font-size: 1.25rem;
  font-weight: 700;
  color: #212529;
  margin: 0 0 0.5rem 0;
  line-height: 1.2;
}

.project-badge {
  background: linear-gradient(135deg, #394d43 0%, #2d3a31 100%);
  color: #ffffff;
  padding: 0.375rem 0.75rem;
  border-radius: 12px;
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.brick-icon {
  font-size: 2rem;
  opacity: 0.7;
  margin-left: 1rem;
}

.brick-card-body {
  padding: 0 1.5rem 1.5rem 1.5rem;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.service-description {
  color: #6c757d;
  margin-bottom: 1.5rem;
  line-height: 1.5;
  flex: 1;
}

.data-flow {
  margin-bottom: 1.5rem;
}

.data-item {
  margin-bottom: 1rem;
  padding: 0.75rem;
  background: #f0f7f4;
  border-radius: 10px;
  border-left: 4px solid #394d43;
}

.data-item:last-child {
  margin-bottom: 0;
}

.data-label {
  font-size: 0.8rem;
  font-weight: 600;
  color: #495057;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  margin-bottom: 0.25rem;
  display: flex;
  align-items: center;
}

.data-icon {
  margin-right: 0.5rem;
}

.data-value {
  font-size: 0.9rem;
  color: #212529;
  font-weight: 500;
}

.tags-section {
  margin-bottom: 1rem;
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.tag-chip {
  background: linear-gradient(135deg, #6f42c1 0%, #5a2d82 100%);
  color: white;
  padding: 0.375rem 0.75rem;
  border-radius: 16px;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.3px;
}

.brick-card-footer {
  padding: 1rem 1.5rem;
  background: #f8f9fa;
  border-top: 1px solid #e9ecef;
}

.creator-info {
  display: flex;
  align-items: center;
}

.creator-avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 0.8rem;
  margin-right: 0.75rem;
}

.creator-details {
  display: flex;
  flex-direction: column;
}

.creator-name {
  font-weight: 600;
  color: #212529;
  font-size: 0.9rem;
}

.creation-date {
  font-size: 0.8rem;
  color: #6c757d;
}

/* List View */
.list-view {
  background: white;
  border-radius: 16px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.06);
  overflow: hidden;
}

.table {
  margin-bottom: 0;
}

.table thead th {
  background: #f8f9fa;
  border-top: none;
  border-bottom: 2px solid #e9ecef;
  font-weight: 700;
  color: #495057;
  text-transform: uppercase;
  font-size: 0.8rem;
  letter-spacing: 0.5px;
  padding: 1rem;
}

.brick-row {
  transition: all 0.2s ease;
}

.brick-row:hover {
  background-color: #f8f9fa;
  transform: scale(1.01);
}

.service-cell {
  font-weight: 600;
  color: #212529;
}

.project-badge-small {
  background: #394d43;
  color: #ffffff;
  padding: 0.25rem 0.5rem;
  border-radius: 8px;
  font-size: 0.75rem;
  font-weight: 600;
}

.description-cell {
  max-width: 250px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  color: #6c757d;
}

.data-flow-cell {
  font-size: 0.8rem;
}

.data-mini {
  margin-bottom: 0.25rem;
  color: #495057;
}

.tags-cell {
  display: flex;
  flex-wrap: wrap;
  gap: 0.25rem;
}

.tag-chip-small {
  background: #6f42c1;
  color: white;
  padding: 0.125rem 0.5rem;
  border-radius: 10px;
  font-size: 0.7rem;
  font-weight: 600;
}

.more-tags {
  background: #6c757d;
  color: white;
  padding: 0.125rem 0.5rem;
  border-radius: 10px;
  font-size: 0.7rem;
  font-weight: 600;
}

.creator-cell {
  display: flex;
  align-items: center;
}

.creator-avatar-small {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  background: #28a745;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 0.7rem;
  margin-right: 0.5rem;
}

.creator-info-small {
  display: flex;
  flex-direction: column;
}

.creator-name-small {
  font-weight: 600;
  color: #212529;
  font-size: 0.8rem;
}

.creation-date-small {
  font-size: 0.7rem;
  color: #6c757d;
}

/* Alerts */
.alert {
  border-radius: 12px;
  border: none;
  padding: 1rem 1.5rem;
}

.alert-info {
  background: linear-gradient(135deg, #cff4fc 0%, #b6effb 100%);
  color: #055160;
  border-left: 4px solid #0dcaf0;
}

.alert-warning {
  background: linear-gradient(135deg, #fff3cd 0%, #ffeaa7 100%);
  color: #664d03;
  border-left: 4px solid #ffc107;
}

.alert-danger {
  background: linear-gradient(135deg, #f8d7da 0%, #f5c2c7 100%);
  color: #721c24;
  border-left: 4px solid #dc3545;
}

/* Responsive Design */
@media (max-width: 991.98px) {
  .filters-card .row .col-lg-2 {
    margin-top: 1rem;
  }
  
  .results-summary .d-flex {
    flex-direction: column;
    gap: 1rem;
  }
  
  .view-options {
    align-self: flex-start;
  }
}

@media (max-width: 767.98px) {
  .brick-card {
    margin-bottom: 1rem;
  }
  
  .brick-card-header {
    padding: 1rem 1rem 0.75rem 1rem;
  }
  
  .brick-card-body {
    padding: 0 1rem 1rem 1rem;
  }
  
  .brick-card-footer {
    padding: 0.75rem 1rem;
  }
  
  .service-name {
    font-size: 1.1rem;
  }
  
  .brick-icon {
    font-size: 1.5rem;
  }
  
  .table-responsive {
    font-size: 0.8rem;
  }
  
  .description-cell {
    max-width: 150px;
  }
}

/* Animation */
.bricks-container {
  animation: fadeIn 0.4s ease-in;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Loading animations */
.brick-card {
  animation: slideUp 0.3s ease-out;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Enhanced hover effects */
.tag-chip:hover {
  transform: scale(1.05);
}

.creator-avatar:hover {
  transform: scale(1.1);
}

.project-badge:hover {
  transform: scale(1.05);
}

/* Edit Mode Styles */
.brick-actions {
  display: flex;
  align-items: flex-start;
  gap: 0.5rem;
}

.action-buttons {
  display: flex;
  gap: 0.25rem;
}

.edit-actions {
  display: flex;
  gap: 0.25rem;
}

.edit-btn, .delete-btn, .save-btn, .cancel-btn {
  width: 32px;
  height: 32px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.8rem;
  border: 1px solid;
  transition: all 0.2s ease;
}

.edit-btn {
  border-color: #394d43;
  color: #394d43;
}

.edit-btn:hover {
  background-color: #394d43;
  color: #ffffff;
  transform: scale(1.1);
}

.delete-btn {
  border-color: #dc3545;
  color: #dc3545;
}

.delete-btn:hover {
  background-color: #dc3545;
  color: white;
  transform: scale(1.1);
}

.save-btn {
  border-color: #198754;
  color: #198754;
}

.save-btn:hover {
  background-color: #198754;
  color: white;
  transform: scale(1.1);
}

.cancel-btn {
  border-color: #6c757d;
  color: #6c757d;
}

.cancel-btn:hover {
  background-color: #6c757d;
  color: white;
  transform: scale(1.1);
}

/* Edit Form Styles */
.service-name-edit {
  font-size: 1.25rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
  border: 2px solid #394d43;
  border-radius: 8px;
}

.project-name-edit {
  font-size: 0.8rem;
  font-weight: 600;
  border: 2px solid #394d43;
  border-radius: 8px;
  padding: 0.375rem 0.5rem;
}

.description-edit {
  border: 2px solid #394d43;
  border-radius: 8px;
  resize: vertical;
  min-height: 80px;
}

.data-edit {
  border: 2px solid #394d43;
  border-radius: 6px;
  font-size: 0.9rem;
  padding: 0.5rem 0.75rem;
}

.tags-edit {
  margin-bottom: 1rem;
}

.form-label-small {
  font-size: 0.8rem;
  font-weight: 600;
  color: #495057;
  margin-bottom: 0.25rem;
  display: block;
}

.tags-input {
  border: 2px solid #394d43;
  border-radius: 8px;
  font-size: 0.85rem;
}

.creator-name-edit {
  font-size: 0.9rem;
  font-weight: 600;
  border: 2px solid #394d43;
  border-radius: 6px;
  padding: 0.375rem 0.5rem;
}

/* List View Edit Styles */
.list-actions {
  display: flex;
  gap: 0.25rem;
  margin-top: 0.5rem;
}

.list-edit-actions {
  display: flex;
  gap: 0.25rem;
}

.creator-edit-cell {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.data-edit-cell {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

/* Hover effects for edit mode cards */
.brick-card:has(.edit-actions) {
  border-color: #394d43;
  box-shadow: 0 4px 12px rgba(57, 77, 67, 0.15);
}

.brick-card:has(.edit-actions):hover {
  transform: none;
}

/* Focus styles for edit inputs */
.form-control:focus {
  border-color: #394d43;
  box-shadow: 0 0 0 0.2rem rgba(57, 77, 67, 0.25);
}

/* Loading states */
.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* Mobile responsiveness for edit mode */
@media (max-width: 767.98px) {
  .brick-actions {
    flex-direction: column;
    align-items: stretch;
  }
  
  .action-buttons, .edit-actions {
    justify-content: center;
  }
  
  .edit-btn, .delete-btn, .save-btn, .cancel-btn {
    width: 40px;
    height: 40px;
    font-size: 1rem;
  }
  
  .service-name-edit {
    font-size: 1.1rem;
  }
  
  .brick-card-header {
    flex-direction: column;
    gap: 1rem;
  }
}

/* Animation for edit mode transition */
@keyframes editModeEnter {
  from {
    opacity: 0;
    transform: scale(0.95);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}

.edit-actions, .edit-form {
  animation: editModeEnter 0.2s ease-out;
}

/* Enhanced visual feedback */
.form-control:invalid {
  border-color: #dc3545;
}

.form-control:valid {
  border-color: #198754;
}

/* Tags display vs edit styling */
.tags-display {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.tags-edit {
  display: flex;
  flex-direction: column;
}
</style>