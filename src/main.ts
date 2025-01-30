import { createApp } from 'vue'
import App from './App.vue'
import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { faPlay, faStop, faBars } from '@fortawesome/free-solid-svg-icons'
import './styles.css'

library.add(faPlay, faStop, faBars)
const app = createApp(App)

app.component('font-awesome-icon', FontAwesomeIcon)

app.mount('#app')
