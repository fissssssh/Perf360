import { useAuthStore } from '@/stores/auth'
import type { App, DirectiveBinding } from 'vue'

const authDirective = {
  created(el: HTMLElement) {
    el.style.visibility = 'hidden'
  },
  mounted(el: HTMLElement, binding: DirectiveBinding<{ role: string }>) {
    const store = useAuthStore()
    if (store.isAuthenticated) {
      if (binding.value.role) {
        const roles = store.currentUserInformation?.roles
        if (roles && roles.includes(binding.value.role)) {
          el.style.visibility = 'visible'
          return
        }
      } else {
        el.style.visibility = 'visible'
        return
      }
    }
    el.parentNode?.removeChild(el)
  },
}

export default {
  install(app: App) {
    app.directive('auth', authDirective)
  },
}
