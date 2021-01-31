<template>
  <footer class="no-print mt-4">
    <div>
      <a href="https://github.com/void-type/foodstuffs">
        FoodStuffs {{ version }} is open source!</a>
    </div>
    <div>
      Logo icon made by <a
        href="http://www.freepik.com/"
        title="Freepik"
      >Freepik</a> from <a
        href="https://www.flaticon.com/"
        title="Flaticon"
      >www.flaticon.com</a>
    </div>
  </footer>
</template>

<script>
import webApi from '@/webApi';

export default {
  data() {
    return {
      version: null,
      showAbout: false,
    };
  },
  created() {
    webApi.app.getVersion(
      (data) => { this.setVersion(data); },
      () => {},
    );
  },
  methods: {
    setVersion(data) {
      let version = `v${data.version}`;

      if (data.isPublicRelease === false) {
        const gitCommitId = data.gitCommitId.slice(0, 10);
        version += `-g${gitCommitId}`;
      }

      this.version = version;
    },
  },
};
</script>

<style lang="scss" scoped>
footer {
  border-top: 1px solid;
  padding: 1em 0rem;
  text-align: center;
}
</style>
