import fs from 'fs';
import path from 'path';
import chokidar from 'chokidar';

// İzlenecek dizin (örneğin, src klasörü)
const watchPath = './src';

// JSX dosyasında className eklendiğinde CSS dosyasına selector ekle
chokidar.watch(watchPath).on('change', (filePath) => {
    if (filePath.endsWith('.jsx')) {
        const cssFilePath = filePath.replace('.jsx', '.css');
        const jsxContent = fs.readFileSync(filePath, 'utf8');

        // className'leri bul
        const classNames = jsxContent.match(/className="([^"]+)"/g);
        if (classNames) {
            const uniqueClassNames = [...new Set(classNames.map(cn => cn.replace('className="', '').replace('"', '')))];

            // CSS dosyasını oku veya oluştur
            let cssContent = fs.existsSync(cssFilePath) ? fs.readFileSync(cssFilePath, 'utf8') : '';

            // Yeni class'ları ekle
            uniqueClassNames.forEach(className => {
                if (!cssContent.includes(`.${className}`)) {
                    cssContent += `\n.${className} {\n  /* Styles for ${className} */\n}\n`;
                }
            });

            // CSS dosyasını güncelle
            fs.writeFileSync(cssFilePath, cssContent);
            console.log(`Updated CSS file: ${cssFilePath}`);
        }
    }
});