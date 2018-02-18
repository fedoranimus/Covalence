export class DistanceValueConverter {
    toView(value) {
        return Math.floor(value).toLocaleString();
    }
}