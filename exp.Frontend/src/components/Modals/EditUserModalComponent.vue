<template>
  <div
    class="modal fade"
    id="editUserModal"
    tabindex="-1"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <Form @submit="EditUser()" ref="editUser" :validation-schema="schema" v-slot="{ errors }">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Edit user</h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <div class="modal-body">
            <div class="separator"></div>
            <div>
              <label for="user-edit-name" class="form-label">Name</label>
              <Field
                type="text"
                v-model="user.Name"
                class="form-control"
                 :class="{ 'border-danger': errors.username }"
                id="user-edit-name"
                placeholder="Name"
                aria-describedby="user-name"
                autocomplete="off"
                name="username"
              ></Field>
              <ErrorMessage name="username" class="error-message"></ErrorMessage>
            </div>

            <div>
              <label class="form-label mt-3">Roles</label>
              <Field name="role" v-model="user.Roles">
                <multiselect
                  class="custom-multiselect"
                  :class="{ 'field-error': errors.role }"
                  v-model="user.Roles"
                  :options="roles"
                  placeholder="Role"
                  selected-label=""
                  deselect-label=""
                  select-label=""
                  label="Name"
                  track-by="Id"
                  :multiple="true"
                  :loading="loaders.loaderMultiselectCountries"
                  @open="GetRoles()"
                ></multiselect
              ></Field>
              <ErrorMessage name="role" class="error-message"></ErrorMessage>
            </div>
          </div>
          <div class="modal-footer px-0 mx-0 row gap-4">
            <div class="col p-0 m-0">
              <button type="button" class="btn btn-outline-secondary w-100" data-bs-dismiss="modal">
                Cancel
              </button>
            </div>
            <div class="col p-0 m-0">
              <button type="submit" class="btn btn-primary btn-md w-100">Save</button>
            </div>
          </div>
        </Form>
      </div>
    </div>
  </div>
</template>
<script>
import { Form, Field, ErrorMessage } from 'vee-validate'
import * as yup from 'yup'
export default {
  name: 'editUserModal',
  components: {
    Form,
    Field,
    ErrorMessage,
  },
  emits: ['updateUsers'],
  props: {
    userToEdit: {
      type: Object,
      default() {
        return {}
      },
    },
  },
  data() {
    return {
      user: {
        Name: null,
        Email: null,
        Roles: null,
      },
      loaders: {},
      roles: [],
    }
  },
  methods: {
    GetRoles() {
      this.roles = []
      this.loaders.loaderMultiselectCountries = true
      this.$axios
        .get(`/api/Administration/getRoles`)
        .then((response) => {
          this.loaders.loaderMultiselectCountries = false
          this.roles = response.data
        })
        .catch(() => {
          this.loaders.loaderMultiselectCountries = false
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    EditUser() {
      const obj = {
        Id:this.user.Id,
        Name: this.user.Name,
        RoleIds: this.user.Roles.map((role) => role.Id),
      }
      this.$axios
        .put(`/api/Administration/updateUser`, obj)
        .then((response) => {
          this.$emit('updateUsers')
          $('#editUserModal').modal('hide')
          this.$utils.ToastNotify('success', 'User has been edited')
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  },

  computed: {
    schema() {
      return yup.object({
        username: yup
          .string()
          .required('This field is required')
          .min(6, 'Minimum 6 characters')
          .max(200, 'Maximum 200 characters'),
        role: yup.array().min(1, 'Select a role').required('Select a role'),
      })
    },
  },
  watch:{
    userToEdit(newVal, oldVal){
        this.user=JSON.parse(JSON.stringify(newVal));

    }
  }
}
</script>
<style scoped>
.separator {
  height: 1px;
  width: 100%;
  background-color: var(--neutral-200);
  margin: 12px 0px 24px 0px;
}

.modal-dialog {
  max-width: 650px;
}
.modal-content {
  border: none;
  padding: 24px;
  border-radius: 12px;
}
.modal-header {
  padding: 0px;
  border-bottom: 0px;
}
.modal-body {
  padding: 0px;
  padding-bottom: 24px;
}

.modal-footer {
  padding-top: 24px;
}
.modal-title {
  font-size: 24px;
  color: var(--neutral-900);
  font-weight: 600 !important;
  text-align: center;
  font-family: 'Roboto';
}
h5 {
  color: var(--neutral-900);
  font-size: 15px;
}
</style>
