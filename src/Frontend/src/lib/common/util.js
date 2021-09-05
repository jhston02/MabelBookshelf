import {v4 as uuidv4} from 'uuid';

export function getGuid() {
    return uuidv4();
}

export function blurOnKey(event) {
    const {code} = event;
    if (code === 'Enter' || code === 'Escape' || code === 'Tab') {
        event.target.blur();
    }
}

export const colorDictionary = {
    yellow: {
        darkText: "text-yellow-600",
        mediumBackground: "bg-yellow-200",
        lightBackground: "bg-yellow-50",
        border: "border-yellow-300",
        complementary: "purple"
    },
    blue: {
        darkText: "text-blue-600",
        mediumBackground: "bg-blue-200",
        lightBackground: "bg-blue-50",
        complementary: "pink"
    },
    red: {
        darkText: "text-red-600",
        mediumBackground: "bg-red-200",
        lightBackground: "bg-red-50",
        border: "border-red-300",
        complementary: "green"
    },
    green: {
        darkText: "text-green-600",
        mediumBackground: "bg-green-200",
        lightBackground: "bg-green-50",
        border: "border-green-300",
        complementary: "red"
    },
    purple: {
        darkText: "text-purple-600",
        mediumBackground: "bg-purple-200",
        lightBackground: "bg-purple-50",
        border: "border-purple-300",
        complementary: "yellow"
    },
    pink: {
        darkText: "text-pink-600",
        mediumBackground: "bg-pink-200",
        lightBackground: "bg-pink-50",
        border: "border-pink-300",
        complementary: "blue"
    }
}