import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import { spawn } from 'child_process';
import fs from 'fs';
import path from 'path';

/**
 * Retrieves the ASP.NET Core dev certificate paths
 */
function getDotnetCertPaths() {
  const baseFolder =
    process.env.APPDATA !== undefined && process.env.APPDATA !== ''
      ? `${process.env.APPDATA}/ASP.NET/https`
      : `${process.env.HOME}/.aspnet/https`;

  const certificateName = process.env.npm_package_name;

  const cert = path.join(baseFolder, `${certificateName}.pem`);
  const key = path.join(baseFolder, `${certificateName}.key`);

  return { baseFolder, cert, key };
}

// https://vitejs.dev/config/
export default defineConfig(async ({ mode }) => {
  const { baseFolder, cert, key } = getDotnetCertPaths();

  // If you have problems with the cert, uncomment these to clean the certs and force their re-gen. Be sure to comment when done.
  // if (fs.existsSync(cert)) {
  //   fs.unlinkSync(cert);
  // }
  // if (fs.existsSync(key)) {
  //   fs.unlinkSync(key);
  // }

  // Ensure the certificate and key exist
  if (mode === 'development' && (!fs.existsSync(cert) || !fs.existsSync(key))) {
    if (!fs.existsSync(baseFolder)) {
      fs.mkdirSync(baseFolder, { recursive: true });
    }

    // Wait for the certificate to be generated
    await new Promise((resolve) => {
      spawn(
        'dotnet',
        ['dev-certs', 'https', '--export-path', cert, '--format', 'Pem', '--no-password'],
        { stdio: 'inherit' }
      ).on('exit', (code) => {
        resolve(null);
        if (code) {
          process.exit(code);
        }
      });
    });
  }

  return {
    plugins: [vue()],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
      },
    },
    build: {
      manifest: true,
      rollupOptions: {
        input: ['./src/main.ts', './src/styles/styles-main.scss'],
      },
      outDir: '../FoodStuffs.Web/wwwroot',
      emptyOutDir: true,
      sourcemap: true,
    },
    watch: {
      include: './src/**',
    },
    server: {
      origin: 'https://localhost:5173',
      strictPort: true,
      https: true && {
        cert,
        key,
      },
    },
  };
});
