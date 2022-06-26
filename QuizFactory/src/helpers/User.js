export const getUsername = () => {
    const userData = JSON.parse(localStorage.getItem('user'));
    if (userData === null)
        return '';
    return userData.username;
}

export const getUserType = () => {
    const userData = JSON.parse(localStorage.getItem('user'));
    if (userData === null)
        return '';
    return userData.role;
}