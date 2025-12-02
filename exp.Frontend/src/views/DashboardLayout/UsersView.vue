<template>
  <div class="container-lg mt-4 p-se-20-px">
    <div class="flex-between-center mb-4">
      <h2 class="color-N-900 fw-600 roboto-cond">Users</h2>
      <button
        type="button"
        class="btn btn-info btn-md"
        data-bs-toggle="modal"
        data-bs-target="#addUserModal"
      >
        <img src="@/assets/images/icon-plus-blue.svg" alt="" />Add user
      </button>
    </div>
    <div class="white-box mb-3">
      <div class="row gutter-12-px">
        <div class="col-12 col-md-6 col-lg-4 col-xl-3">
          <label for="search-text-users" class="form-label">Search</label>
          <div class="input-group flex-nowrap">
            <span class="input-group-text"
              ><img src="@/assets/images/search.svg" alt="search" /></span
            ><input
              v-model="filter.SearchText"
              class="form-control"
              type="text"
              id="search-text-users"
              placeholder="Search users"
              aria-describedby="search-user"
              autocomplete="off"
              @keydown.enter="GetUsers()"
            />
          </div>
        </div>
        <div class="col-12 col-md-6 col-lg-4 col-xl-3 mt-3 mt-md-0">
          <div class="position-relative">
            <label for="search-text-users" class="form-label">Roles</label>
            <multiselect
              class="custom-multiselect"
              v-model="filter.Roles"
              :options="roles"
              placeholder="Search roles"
              selected-label=""
              deselect-label=""
              select-label=""
              label="Name"
              track-by="Id"
              :allow-empty="false"
              :loading="loaders.loaderMultiselectRoles"
              @select="GetUsers()"
              @open="GetRoles()"
            ></multiselect>

            <img
              v-if="filter.Roles != null"
              src="/src/assets/images/x-reset.svg"
              alt=""
              class="button-reset-multiselect"
              @click="((filter.Roles = null), GetUsers())"
            />
          </div>
        </div>
        <div class="col"></div>
        <div class="col-auto mt-3 mt-lg-0">
          <div class="h-100 d-flex align-items-end gap-2">
            <button type="button" @click="GetUsers()" class="btn action">
              <img src="@/assets/images/filter.svg" alt="" />
            </button>
            <button type="button" @click="ResetFilters()" class="btn action">
              <img src="@/assets/images/delete.svg" alt="" />
            </button>
          </div>
        </div>
      </div>
    </div>
    <div class="white-box px-2 pt-2 mb-5">
      <div class="table-responsive">
        <table class="custom table">
          <thead>
            <tr>
              <th width="8"></th>
              <th width="30%">
                <span
                  @click="OrderBy('Name')"
                  class="pointer flex-align-center"
                  :class="{
                    'active-order-by': filter.OrderBy.includes('Name'),
                  }"
                >
                  <OrderByComponent value="Name" :orderBy="filter.OrderBy"></OrderByComponent>Name
                </span>
              </th>
              <th width="30%">Email</th>
              <th>Role</th>
              <th></th>
              <th width="1"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(user, index) in users.Items" :key="index">
              <td></td>
              <td class="fw-600 color-N-900">{{ user.Name }}</td>
              <td>{{ user.Email }}</td>
              <td>
                <span>{{user.Roles.join(', ')}}</span>
              </td>
              <td class="pe-0">
                <div class="d-flex justify-content-end gap-2">
                  <button type="button" @click="OpenModalEditUser(user.Id)" class="btn btn-secondary btn-sm">
                   Edit
                  </button>
                  <button type="button" @click="DeleteUser(user.Id)" class="btn btn-danger btn-sm">
                    Delete
                  </button>
                </div>
              </td>
              <td></td>
            </tr>
          </tbody>
        </table>
      </div>

      <LoaderTableComponent v-if="!loaders.areUsersLoaded"></LoaderTableComponent>
      <NotFoundComponent
        text="No user found"
        v-if="loaders.areUsersLoaded && users.Items.length == 0"
      ></NotFoundComponent>
      <div
        class="py-4 mx-4 d-flex align-items-center justify-content-center justify-content-sm-between wrap"
      >
        <h6 v-if="users.Count > 0" class="color-grey fw-600 mb-3 mb-sm-0">
          Showing
          {{ filter.PageSize > users.Count ? users.Count : users.Items.length }}
          of {{ users.Count }}
          {{ users.Count !== 1 ? 'entries' : 'entry' }}
        </h6>
        <div class="d-flex align-items-center gap-4">
          <PaginationComponent
            @pagechanged="GetUsers"
            :totalPages="users.NumberOfPages"
            :currentPage="filter.PageNumber"
          ></PaginationComponent>
        </div>
      </div>
    </div>
  </div>
  <EditCompanyModalComponent
    @updateOrganisation="GetOrganisation()"
    :copyCompany="userToEdit"
  ></EditCompanyModalComponent>
  <AddUserModalComponent @updateUsers="GetUsers()"></AddUserModalComponent>
  <EditUserModalComponent
    @updateUsers="GetUsers()"
    :userToEdit="userToEdit"
  ></EditUserModalComponent>
</template>
<script>
import PaginationComponent from '@/components/Others/PaginationComponent.vue'

import OrderByComponent from '@/components/Others/OrderByComponent.vue'
import EditCompanyModalComponent from '@/components/Modals/EditCompanyModalComponent.vue'
import NotFoundComponent from '@/components/Others/NotFoundComponent.vue'
import LoaderTableComponent from '@/components/Others/loaders/LoaderTableComponent.vue'
import AddUserModalComponent from '@/components/Modals/AddUserModalComponent.vue'
import EditUserModalComponent from '@/components/Modals/EditUserModalComponent.vue'
export default {
  name: 'Users',
  components: {
    PaginationComponent,
    EditCompanyModalComponent,
    NotFoundComponent,
    OrderByComponent,
    LoaderTableComponent,
    AddUserModalComponent,
    EditUserModalComponent,
  },
  data() {
    return {
      filter: {
        PageSize: 6,
        PageNumber: 1,
        OrderBy: 'Name',
        SearchText: '',
        Roles: null,
      },
      loaders: {},
      userToEdit: {},
      users: {
        NumberOfPages: 1,
        Count: null,
        Items: [],
      },
      roles: [],
    }
  },
  methods: {
    ResetFilters() {
      this.filter.SearchText = ''
      this.filter.Roles = null
      this.GetUsers()
    },
    OrderBy(orderBy) {
      const value = this.filter.OrderBy.split('_')
      if (value[0] === orderBy) {
        if (!this.filter.OrderBy.includes('_desc')) {
          this.filter.OrderBy = `${orderBy}_desc`
        } else {
          this.filter.OrderBy = orderBy
        }
      } else {
        this.filter.OrderBy = `${orderBy}_desc`
      }
      this.GetUsers()
    },
    GetRoles() {
      this.roles = []
      this.loaders.loaderMultiselectRoles = true
      this.$axios
        .get(`/api/Administration/getRoles`)
        .then((response) => {
          this.loaders.loaderMultiselectRoles = false
          this.roles = response.data
        })
        .catch(() => {
          this.loaders.loaderMultiselectRoles = false
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    OpenModalEditUser(userId) {
      this.$store.commit('SET_LOADER', true)
      this.$axios
        .get(`/api/Administration/getUserForEdit?userId=${userId}`)
        .then((response) => {
          this.$store.commit('SET_LOADER', false)
          this.userToEdit = response.data
          $('#editUserModal').modal('show')
        })
        .catch(() => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    DeleteUser(userId) {
      this.$swal
        .fire({
          title: 'Are you sure you want to delete this user?',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#1B7DF5',
          cancelButtonColor: '#DF0C3D',
          confirmButtonText: 'Yes',
        })
        .then((result) => {
          if (result.isConfirmed) {
            this.$store.commit('SET_LOADER', true)
            this.$axios
              .delete(`/api/Administration/deleteUser?userId=${userId}`)
              .then((response) => {
                this.GetUsers()
                this.$store.commit('SET_LOADER', false)
                this.$utils.ToastNotify('success', 'User has been deleted')
              })
              .catch((error) => {
                this.$store.commit('SET_LOADER', false)
                this.$utils.ToastNotifySomethingWentWrong()
              })
          }
        })
    },
    GetUsers(page) {
      this.loaders.areUsersLoaded = false
      this.filter.PageNumber = 1
      if (page) {
        this.filter.PageNumber = page
      }
      this.users = {
        NumberOfPages: 1,
        Count: null,
        Items: [],
      }
      const body = {
        PageSize: this.filter.PageSize,
        PageNumber: this.filter.PageNumber,
        OrderBy: this.filter.OrderBy,
        ...(this.filter.SearchText && { SearchText: this.filter.SearchText }),
        ...(this.filter.Roles && { RoleId: this.filter.Roles.Id }),
      }
      this.$axios
        .post(`/api/Administration/getUsers`, body)
        .then((response) => {
          this.users = response.data
          this.loaders.areUsersLoaded = true
        })
        .catch(() => {
          this.loaders.areUsersLoaded = true
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  },
  created() {
    this.GetUsers()
  },
}
</script>
<style scoped>

hr {
  color: #d9d9d9;
  opacity: 1;
  margin-bottom: 24px;
}
</style>
