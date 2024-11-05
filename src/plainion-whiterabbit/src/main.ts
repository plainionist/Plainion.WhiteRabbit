import { createApp } from 'vue'
import App from './App.vue'
import 'tailwindcss/tailwind.css'
import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { faPlay, faStop } from '@fortawesome/free-solid-svg-icons'
import 'tailwindcss/base.css'
import 'tailwindcss/components.css'
import 'tailwindcss/utilities.css'

library.add(faPlay, faStop)
const app = createApp(App)

app.component('font-awesome-icon', FontAwesomeIcon)

app.mount('#app')
