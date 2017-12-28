export class ConnectionStateValueConverter {
    toView(value) {
        switch(value) {
            case 0:
                return "Pending";
            case 1:
                return "Connected";
            case 2:
                return "Blocked";
        }
    }
}