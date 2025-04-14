import { useAuthStore } from '@/stores/auth'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/LoginView.vue'),
      props: true,
    },
    {
      path: '/',
      component: () => import('@/views/Home/HomeView.vue'),
      meta: { auth: true },
      redirect: '/dashboard',
      children: [
        {
          path: 'dashboard',
          children: [
            {
              path: '',
              name: 'dashboard',
              component: () => import('@/views/Home/Dashboard/DashboardView.vue'),
            },
            {
              path: 'my-reviews/:id',
              name: 'my-review',
              component: () => import('@/views/Home/Dashboard/MyReviewView.vue'),
              props: true,
            },
          ],
        },
        {
          path: 'admin',
          meta: { auth: true, roles: ['admin'] },
          children: [
            {
              path: 'reviews',
              children: [
                {
                  path: '',
                  name: 'reviews',
                  component: () => import('@/views/Home/Admin/Reviews/ReviewsView.vue'),
                },
                {
                  path: ':id',
                  name: 'review',
                  component: () => import('@/views/Home/Admin/Reviews/ReviewView.vue'),
                  props: (route) => ({ id: Number(route.params.id) }),
                },
              ],
            },
            {
              path: 'users',
              name: 'users',
              component: () => import('@/views/Home/Admin/Users/UsersView.vue'),
            },
            {
              path: 'review-roles',
              children: [
                {
                  path: '',
                  name: 'review-roles',
                  component: () => import('@/views/Home/Admin/ReviewRoles/ReviewRolesView.vue'),
                },
                {
                  path: ':id/review-dimensions',
                  component: () => import('@/views/Home/Admin/ReviewRoles/ReviewDimensions.vue'),
                  props: true,
                },
              ],
            },
          ],
        },
      ],
    },
  ],
})

router.beforeEach((to, from) => {
  const authStore = useAuthStore()
  if (!authStore.isAuthenticated && to.meta['auth']) {
    return { name: 'login', params: { returnUrl: from.path } }
  } else if (authStore.isAuthenticated) {
    if (to.name === 'login') {
      return { path: '/' }
    } else if (to.meta['auth'] && to.meta['roles']) {
      const requireRoles = to.meta['roles'] as string[]
      if (!authStore.currentUserInformation?.roles.some((r) => requireRoles.includes(r))) {
        ElMessage.error({ message: 'Forbidden' })
        return false
      }
    }
  }
})

export default router
