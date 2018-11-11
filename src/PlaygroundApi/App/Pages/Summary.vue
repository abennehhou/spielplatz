<template>
    <div>
        <p>Summary</p>
        <v-data-table :headers="headers"
                      :items="items"
                      :loading="loading"
                      class="elevation-1">
            <v-progress-linear slot="progress" color="blue" indeterminate></v-progress-linear>
            <template slot="items" slot-scope="props">
                <td>{{ props.item.id }}</td>
                <td>{{ props.item.name}}</td>
                <td>{{ props.item.owner }}</td>
                <td>{{ props.item.description }}</td>
                <td>{{ props.item.tags ? props.item.tags.join(', ') : null }}</td>
            </template>
        </v-data-table>
    </div>

</template>
<script>
    import axios from 'axios';
    export default {
        name: 'Summary',
        data() {
            return {
                message: 'Summary Page',
                loading: false,
                items: [],
                headers: [
                    { text: 'Id', value: 'id' },
                    { text: 'Name', value: 'name' },
                    { text: 'Owner', value: 'owner' },
                    { text: 'Description', value: 'description' },
                    { text: 'Tags', value: 'tags' }
                ]
            };
        },
        mounted() {
            this.getItems();
        },
        methods: {
            getItems() {
                this.loading = true;
                axios.get(`api/Items`)
                    .then(response => {
                        this.items = response.data.items;
                        this.loading = false;
                    })
                    .catch(e => {
                        console.log(e);
                        this.loading = false;
                    });
            }
        }
    };
</script>