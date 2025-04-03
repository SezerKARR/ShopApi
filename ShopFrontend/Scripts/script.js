import fs from 'fs';
import path from 'path';
import chokidar from 'chokidar';

const watchPath = './src';

chokidar.watch(watchPath).on('change', (filePath) => {
    if (filePath.endsWith('.jsx')) {
        const cssFilePath = filePath.replace('.jsx', '.css');
        const jsxContent = fs.readFileSync(filePath, 'utf8');

        const classNames = jsxContent.match(/className="([^"]+)"/g);
        if (classNames) {
            const uniqueClassNames = [...new Set(classNames.map(cn => cn.replace('className="', '').replace('"', '')))];

            let cssContent = fs.existsSync(cssFilePath) ? fs.readFileSync(cssFilePath, 'utf8') : '';

            uniqueClassNames.forEach(className => {
                if (!cssContent.includes(`.${className}`)) {
                    cssContent += `\n.${className} {\n  /* Styles for ${className} */\n}\n`;
                }
            });

            fs.writeFileSync(cssFilePath, cssContent);
        }
    }
});