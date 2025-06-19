<template>
  <div class="form-container">
    <div class="row justify-content-center">
      <div class="col-xl-10">
        <div class="card form-card">
          <div class="card-body">
            
            <!-- Success message -->
            <div v-if="successMessage" class="alert alert-success">
              {{ successMessage }}
            </div>
            
            <!-- Error message -->
            <div v-if="errorMessage" class="alert alert-danger">
              {{ errorMessage }}
            </div>
            
            <form @submit.prevent="submitForm">
              <div class="row">
                <!-- Service Name -->
                <div class="col-md-6 mb-3">
                  <label for="serviceName" class="form-label">Service/Feature Name *</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="serviceName"
                    v-model="form.serviceName"
                    required
                    placeholder="e.g., User Authentication"
                  >
                </div>
                
                <!-- Project Name -->
                <div class="col-md-6 mb-3">
                  <label for="projectName" class="form-label">Project Name *</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="projectName"
                    v-model="form.projectName"
                    required
                    placeholder="e.g., Customer Portal"
                  >
                </div>
              </div>
              
              <!-- Description -->
              <div class="mb-3">
                <label for="description" class="form-label">Description *</label>
                <textarea 
                  class="form-control" 
                  id="description"
                  v-model="form.description"
                  rows="3"
                  required
                  placeholder="Brief description of what this service does (2-3 sentences)"
                ></textarea>
              </div>
              
              <div class="row">
                <!-- Data Consumed -->
                <div class="col-md-6 mb-3">
                  <label for="dataConsumed" class="form-label">Data Consumed</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="dataConsumed"
                    v-model="form.dataConsumed"
                    placeholder="e.g., username, password, session tokens"
                  >
                </div>
                
                <!-- Data Produced -->
                <div class="col-md-6 mb-3">
                  <label for="dataProduced" class="form-label">Data Produced</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="dataProduced"
                    v-model="form.dataProduced"
                    placeholder="e.g., authentication status, user profile"
                  >
                </div>
              </div>
              
              <div class="row">
                <!-- Tags -->
                <div class="col-md-6 mb-3">
                  <label for="tags" class="form-label">Tags</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="tags"
                    v-model="form.tags"
                    placeholder="e.g., auth, security, user-management"
                  >
                  <small class="form-text text-muted">Separate multiple tags with commas</small>
                </div>
                
                <!-- Creator Name -->
                <div class="col-md-6 mb-3">
                  <label for="creatorName" class="form-label">Your Name *</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="creatorName"
                    v-model="form.creatorName"
                    required
                    placeholder="Your name"
                  >
                </div>
              </div>
              
              <!-- Submit Button -->
              <div class="form-actions">
                <div class="row">
                  <div class="col-md-6">
                    <button 
                      type="button" 
                      class="btn btn-outline-secondary btn-lg w-100"
                      @click="resetForm"
                      :disabled="submitting"
                    >
                      Reset Form
                    </button>
                  </div>
                  <div class="col-md-6">
                    <button 
                      type="submit" 
                      class="btn btn-primary btn-lg w-100"
                      :disabled="submitting"
                    >
                      <span v-if="submitting" class="spinner-border spinner-border-sm me-2" role="status"></span>
                      {{ submitting ? 'Creating Brick...' : 'Create Brick' }}
                    </button>
                  </div>
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'BrickForm',
  data() {
    return {
      form: {
        serviceName: '',
        projectName: '',
        description: '',
        dataConsumed: '',
        dataProduced: '',
        tags: '',
        creatorName: ''
      },
      submitting: false,
      successMessage: '',
      errorMessage: ''
    }
  },
  methods: {
    async submitForm() {
      try {
        this.submitting = true
        this.errorMessage = ''
        this.successMessage = ''
        
        // Make API call to create brick
        await axios.post('http://localhost:5213/api/bricks', this.form)
        
        // Show success message
        this.successMessage = `Brick "${this.form.serviceName}" created successfully!`
        
        // Clear form
        this.resetForm()
        
        // Emit event to parent to refresh brick list
        this.$emit('brick-created')
        
      } catch (error) {
        console.error('Error creating brick:', error)
        this.errorMessage = 'Failed to create brick. Please try again.'
      } finally {
        this.submitting = false
      }
    },
    
    resetForm() {
      this.form = {
        serviceName: '',
        projectName: '',
        description: '',
        dataConsumed: '',
        dataProduced: '',
        tags: '',
        creatorName: ''
      }
    }
  }
}
</script>

<style scoped>
.form-container {
  max-width: 100%;
}

.form-card {
  border: none;
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
  border-radius: 16px;
}

.form-actions {
  margin-top: 2rem;
  padding-top: 2rem;
  border-top: 1px solid #e9ecef;
}

.alert {
  border-radius: 12px;
  border: none;
  padding: 1rem 1.5rem;
}

.alert-success {
  background-color: #d1edff;
  color: #0c5460;
  border-left: 4px solid #0dcaf0;
}

.alert-danger {
  background-color: #f8d7da;
  color: #721c24;
  border-left: 4px solid #dc3545;
}

.form-label {
  font-weight: 600;
  color: #495057;
  margin-bottom: 0.5rem;
}

.form-control, .form-select {
  border-radius: 8px;
  border: 1px solid #ced4da;
  padding: 0.75rem 1rem;
  font-size: 0.95rem;
  transition: all 0.2s ease;
}

.form-control:focus, .form-select:focus {
  border-color: #394d43;
  box-shadow: 0 0 0 0.2rem rgba(57, 77, 67, 0.25);
}

.btn {
  border-radius: 10px;
  font-weight: 600;
  padding: 0.875rem 1.5rem;
  transition: all 0.2s ease;
  border: 2px solid transparent;
}

.btn:hover:not(:disabled) {
  transform: translateY(-1px);
}

.btn-primary {
  background: linear-gradient(135deg, #394d43 0%, #2d3a31 100%);
  border-color: #394d43;
  color: #ffffff;
}

.btn-primary:hover:not(:disabled) {
  background: linear-gradient(135deg, #2d3a31 0%, #1f261c 100%);
  box-shadow: 0 4px 12px rgba(57, 77, 67, 0.3);
  color: #ffffff;
}

.btn-outline-secondary {
  border-color: #7e8d85;
  color: #7e8d85;
}

.btn-outline-secondary:hover:not(:disabled) {
  background-color: #7e8d85;
  border-color: #7e8d85;
  color: white;
}

.form-text {
  font-size: 0.875rem;
  color: #6c757d;
}

.spinner-border-sm {
  width: 1rem;
  height: 1rem;
}

/* Enhanced form layout */
.row.mb-3 {
  margin-bottom: 1.5rem !important;
}

/* Input focus animations */
.form-control:focus {
  transform: scale(1.01);
}

/* Mobile responsiveness */
@media (max-width: 767.98px) {
  .form-actions .row .col-md-6 {
    margin-bottom: 1rem;
  }
  
  .form-actions .row .col-md-6:last-child {
    margin-bottom: 0;
  }
}
</style>