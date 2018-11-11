﻿import Vue from 'vue';
import VueRouter from 'vue-router';
import Summary from './Pages/Summary.vue';
import PageA from './Pages/PageA.vue';
import PageB from './Pages/PageB.vue';
const routes = [
    { path: '/', component: Summary },
    { path: '/pagea', component: PageA },
    { path: '/pageb', component: PageB },
]
Vue.use(VueRouter);
const router = new VueRouter({ mode: 'history', routes: routes });
export default router;
