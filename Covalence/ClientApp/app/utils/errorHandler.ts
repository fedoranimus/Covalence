export class ErrorHandler {
    static formatError(errorMessage: string) {
        return errorMessage.substr(1).slice(0, -1);
    }
}