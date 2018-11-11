<template>
    <v-app>
        <v-navigation-drawer :mini-variant.sync="mini" permanent app>
            <v-list>
                <v-list-tile v-for="item in items"
                             :key="item.title"
                             :to="item.path">
                    <v-list-tile-action>
                        <v-icon>{{ item.icon }}</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>
                        <v-list-tile-title>{{ item.title }}</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>
            </v-list>
        </v-navigation-drawer>
        <v-toolbar app>
            <v-toolbar-side-icon @click.stop="mini = !mini">
            </v-toolbar-side-icon>
            <v-toolbar-title>PlayGround</v-toolbar-title>
            <v-spacer></v-spacer>
        </v-toolbar>
        <v-content>
            <v-container fluid>
                <router-view></router-view>
            </v-container>
        </v-content>
        <v-footer app></v-footer>
    </v-app>


</template>
<script>
    export default {
        data() {
            return {
                mini: false,
                items: [
                    { title: 'Home', icon: 'dashboard', path: '/' },
                    { title: 'Page A', icon: 'looks_one', path: '/pageA' },
                    { title: 'Page B', icon: 'looks_two', path: '/pageB' }
                ],
            };
        }
    };
</script>
