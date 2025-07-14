export function getStorageItem<T>(key: string): T | null {
    const item = localStorage.getItem(key);
    if (item) {
        try {
            return JSON.parse(item) as T;
        } catch (e) {
            console.error(`Error parsing JSON from localStorage for key "${key}":`, e);
            return null;
        }
    }
    return null;
}

export function setStorageItem<T>(key: string, value: T): void {
    try {
        localStorage.setItem (key, JSON.stringify(value));
    } catch (e) {
        console.error(`Error setting item in localStorage for key "${key}":`, e);
    }
}


export function removeStorageItem(key: string): void {
    try {
        localStorage.removeItem(key);
    } catch (e) {
        console.error(`Error removing item from localStorage for key "${key}":`, e);
    }
}


export function clearStorage(): void {
    try {
        localStorage.clear();
    } catch (e) {
        console.error('Error clearing localStorage:', e);
    }
}