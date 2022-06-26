export const addMinToDate = (date, min) => {
    date = new Date(date);
    return new Date(date.getTime() + min * 60000);
}

export const getCurrDate = () => {
    return new Date();
}

export const formatDate = (date) => {
    let tempdate = new Date(date);
    let day = tempdate.getDate();
    let month = tempdate.getMonth();
    let year = tempdate.getFullYear();
    let hour = tempdate.getHours();
    let min = tempdate.getMinutes();

    return day + '/' + month + '/' + year + " " + hour + ":" + min;
}

export const getDateFromString = (date) => {
    return new Date(date);
}