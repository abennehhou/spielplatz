<template>
    <div>
        <p>Page B</p>
        {{message}}
    </div>
</template>
<script>
    export default {
        name: 'PageB',
        data() {
            return {
                message: 'Data from Page B'
            };
        }
    };
</script>