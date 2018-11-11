import Vue from 'vue'
import Vuetify from 'vuetify'
import App from './App.vue';
import Router from './router';
import 'vuetify/dist/vuetify.min.css'
import 'babel-polyfill'

Vue.use(Vuetify)
new Vue({
    el: '#app',
    template: '<App/>',
    components: { App },
    router: Router
});
