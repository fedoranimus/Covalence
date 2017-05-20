import {autoinject, bindable, customElement} from 'aurelia-framework';
import {DOM} from 'aurelia-pal';

@autoinject
@customElement('tag-editor')
export class TagEditorCustomElement {

    constructor(private element: Element, private existingTags: Element, private tagInput: Element) {

    }

    /******************************
     * Bindings
     ******************************/
    @bindable({ attribute: "available-tags" }) availableTags = [];
    @bindable({ attribute: "tags" }) tags = [];
    @bindable({ attribute: "placeholder" }) placeholder = "";

    @bindable({ attribute: "debug" }) debug = false;

    @bindable tagQuery = '';

    @bindable suggestedTags = [];

    private mutationObserver: MutationObserver;

    get inputPlaceholder() {
        let placeholder = this.placeholder;
        if(this.tags.length > 0) {
            placeholder = "";
        }

        return placeholder;
    }

    get hasSuggestions() {
        return this.suggestedTags.length > 0 ? true : false;
    }


    /*****************************
     * Events
     *****************************/

    private addTag(tag: string) {
        if(this.tags.indexOf(tag) === -1 && this.tags.length < 6)
        {
            this.tags.push(tag);
            this.tagQuery = "";
            this.emitEvent(tag, "add");
        }
    }

    private removeTag(tag: string) {
        let index = this.tags.indexOf(tag);
        if(index > -1) {
            this.tags.splice(index, 1);
            this.emitEvent(tag, "remove");
        }   
    }

    private emitEvent(value: string, eventType: string) {
        let event;
        let myWindow = window as any;
        if(myWindow.CustomEvent) {
            event = new CustomEvent(eventType, {
                detail: {
                    value: value
                },
                bubbles: true
            });
        } else {
            event = document.createEvent('CustomEvent');
            event.initCustomEvent(eventType, true, true, {
                detail: {
                    value: value
               }
            });
        }
        this.element.dispatchEvent(event);
    }

    private onKeyPress(event) {
        if(event.which === 13) {
            this.addTag(this.tagQuery);
        }

        return true;
    }


    /******************************
     * Lifecycle
     ******************************/

    bind() {
        this.placeholder = this.placeholder || this.element.getAttribute('placeholder');
    }

    attached() {
        this.mutationObserver = DOM.createMutationObserver((mutations) => {
            let tagsWidth = this.existingTags.getBoundingClientRect().width;
            (this.tagInput as HTMLInputElement).style.width = this.element.clientWidth - tagsWidth + 'px';
        });

        var config = { attributes: true, childList: true, characterData: true };

        this.mutationObserver.observe(this.existingTags, config);
    }

    detached() {
        this.mutationObserver.disconnect();
    }

    /******************************
     * Delegates
     ******************************/

    tagQueryChanged(query: string) {
        if(this.debug) console.log(`Query: ${query}`);
        if(query.length > 0) {
            this.suggestedTags = this.filterTags(query);
        } else {
            this.suggestedTags = [];
        }
    }

    private filterTags(value: string): string[] {
        let matchedTags = [];
        let regexp = new RegExp(value, "i");

        for(let availableTag of this.availableTags) {
            if(matchedTags.length > 4) {
                break;
            } else {
                if(regexp.test(availableTag.name) && this.tags.indexOf(availableTag.id) === -1)
                    matchedTags.push(availableTag);
            }
        };

        if(this.debug) console.log("Matched Tags: ", matchedTags);

        return matchedTags;
    }
}