<template>
    <div>
        <p>Page A</p>
        {{message}}
    </div>
</template>
<script>
    export default {
        name: 'PageA',
        data() {
            return {
                message: 'Data from Page A'
            };
        }
    };
</script>