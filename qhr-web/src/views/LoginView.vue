<script setup lang="ts">
import Message from 'primevue/message'
import InputText from 'primevue/inputtext'
import Fieldset from 'primevue/fieldset'
import Button from 'primevue/button'
import Password from 'primevue/password'

import { ref } from 'vue'
const username = ref(''),
  password = ref()

const error = ref<string>('Fucked up')
</script>

<template>
  <main>
    <form>
      <Fieldset legend="Log in" class="fs-login">
        <label>
          <span>Username</span>
          <InputText v-model="username" />
        </label>
        <label>
          <span>Password</span>
          <Password toggle-mask v-model="password" :feedback="false" />
        </label>
        <Button>Log in</Button>
      </Fieldset>
      <Message v-if="error" severity="error">{{ error }}</Message>
    </form>
  </main>
</template>

<style scoped>
main {
  display: grid;
  min-height: 100vh;
  place-items: center;
}

form {
    display: grid;
    gap: 0.5rem;
}

label {
  display: flex;
  flex-direction: column;
  margin: 0.5rem 0;
}
</style>
