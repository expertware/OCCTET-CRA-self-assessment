import { createStore } from "vuex";
import { $axios } from "../axios/axios-config";


const store = createStore({
  state: {
    user: {
      UserRoles: null,
      UserName: null,
      UserId: null,
      Name:"",
      ImageUrl:null,
    },
    isLoading: false,
  },
  getters: {
    getUser: (state) => state.user,
  },

  actions: {
    async getLoggedUser({ commit }) {
      await $axios
        .get(`/api/Auth/user-details`)
        .then((response) => {
          if (response.status == 200) {
            commit("SET_USERS", response.data);
          }
        })
        .catch(() => {
        });
    },
    removeLoggedUser({ commit }) {
      commit("SET_USERS", {
        UserRole: null,
        UserName: null,
        UserId: null,
      },);
    },
  },
  mutations: {
    SET_LOADER(state, value) {
      state.isLoading = value;
    },
    TOGGLE_LOADER(state) {
      state.isLoading = !state.isLoading;
    },
    SET_USERS(state, user) {
      state.user = user;
    },
    
  },
});

export default store;