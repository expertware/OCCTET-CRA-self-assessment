<template>
  <ul v-if="totalPages > 1" class="pagination">
    <li class="pagination-item">
      <button type="button" @click="OnClickFirstPage" :disabled="IsInFirstPage">
        <img
          src="@/assets/images/arrow-pagination-double.png"
          alt="first-page"
          style="rotate: 180deg"
        />
      </button>
    </li>

    <li class="pagination-item">
      <button type="button" @click="OnClickPreviousPage" :disabled="IsInFirstPage">
        <img
          src="@/assets/images/arrow-pagination.png"
          alt="previous-page"
          style="rotate: 180deg"
        />
      </button>
    </li>

    <!-- Visible Buttons Start -->

    <li v-for="page in pages" :key="page.name" class="pagination-item">
      <button
        type="button"
        @click="OnClickPage(page.name)"
        :disabled="page.isDisabled"
        :class="{ active: IsPageActive(page.name) }"
      >
        {{ page.name }}
      </button>
    </li>

    <!-- Visible Buttons End -->

    <li class="pagination-item">
      <button type="button" @click="OnClickNextPage" :disabled="IsInLastPage">
        <img src="@/assets/images/arrow-pagination.png" alt="next-page" />
      </button>
    </li>

    <li class="pagination-item">
      <button type="button" @click="OnClickLastPage" :disabled="IsInLastPage">
        <img src="@/assets/images/arrow-pagination-double.png" alt="last-page" />
      </button>
    </li>
  </ul>

  <!-- <ul v-if="totalPages<=1" class="space-no-pagination">

  </ul> -->
</template>

<script>
export default {
  emits: ['pagechanged'],
  props: {
    totalPages: {
      type: Number,
      required: true,
    },
    currentPage: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      perPage: 4,
    }
  },
  methods: {
    OnClickFirstPage() {
      this.$emit('pagechanged', 1)
    },

    OnClickPreviousPage() {
      this.$emit('pagechanged', this.currentPage - 1)
    },
    OnClickPage(page) {
      this.$emit('pagechanged', page)
    },
    OnClickNextPage() {
      this.$emit('pagechanged', this.currentPage + 1)
    },

    OnClickLastPage() {
      this.$emit('pagechanged', this.totalPages)
    },

    IsPageActive(page) {
      return this.currentPage === page
    },
  },
  computed: {
    maxVisibleButtons() {
      if (this.totalPages === 2) {
        return 2
      } else {
        return 3
      }
    },

    startPage() {
      // When on the first page
      if (this.currentPage === 1) {
        return 1
      }

      // When on the last page
      if (this.currentPage === this.totalPages) {
        return this.totalPages - (this.maxVisibleButtons - 1)
      }

      // When inbetween
      return this.currentPage - 1
    },
    pages() {
      const range = []

      for (
        let i = this.startPage;
        i <= Math.min(this.startPage + this.maxVisibleButtons - 1, this.totalPages);
        i++
      ) {
        range.push({
          name: i,
          isDisabled: i === this.currentPage,
        })
      }

      return range
    },
    IsInFirstPage() {
      return this.currentPage === 1
    },
    IsInLastPage() {
      return this.currentPage === this.totalPages
    },
  },
}
</script>

<style scoped>
.pagination {
  list-style-type: none;
  gap: 8px;
  justify-content: center;
}

.pagination-item {
  display: inline-block;
}

button:not(.active):disabled {
  color: var(--lynch-500);
  opacity: 0.5;
  border: none;
}
button .active:disabled {
  color: var(--lynch-500);
}

button {
  width: 24px;
  height: 24px;
  display: flex;
  justify-content: center;
  align-items: center;
  border: none;
  border-radius: 6px;
  background-color: #ffffff;
  font-size: 13px;
  font-weight: 600;
  line-height: 104%;
  color: var(--neutral-600);
}

button:hover {
  border: 1px solid var(--neutral-300);
}

.active {
  background-color: var(--primary-500);
  color: #ffffff;
  opacity: 1;
}

@media (max-width: 768px) {
  .btn {
    font-size: 14px;
  }
}
</style>
