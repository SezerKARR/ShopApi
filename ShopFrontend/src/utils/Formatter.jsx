import React from 'react';

export const TLFormatter = new Intl.NumberFormat('tr-TR', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
});


export const formatAsTL = (value) => {
    if (typeof value !== 'number') return ''; 
    return TLFormatter.format(value);
};