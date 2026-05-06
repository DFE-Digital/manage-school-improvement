const fs = require('fs');
const path = require('path');

const filePath = path.resolve('node_modules/@ministryofjustice/frontend/moj/components/cookie-banner/_cookie-banner.scss');

// Read the SCSS file
fs.readFile(filePath, 'utf8', (err, data) => {
    if (err) {
        console.error('Error reading SCSS file:', err);
        return;
    }

    // Modify the import path by removing the node_modules/ part
    let result = data.replace(
        '@import "node_modules/govuk-frontend/dist/govuk/objects/width-container";',
        '@import "govuk-frontend/dist/govuk/objects/width-container";'
    );
    
    // Also handle @forward directives
    result = result.replace(
        '@forward "node_modules/govuk-frontend/dist/govuk/objects/width-container"',
        '@forward "govuk-frontend/dist/govuk/objects/width-container"'
    );

    // Write the modified content back to the file
    fs.writeFile(filePath, result, 'utf8', (err) => {
        if (err) {
            console.error('Error writing SCSS file:', err);
        } else {
            console.log('SCSS import path fixed successfully.');
        }
    });
});
