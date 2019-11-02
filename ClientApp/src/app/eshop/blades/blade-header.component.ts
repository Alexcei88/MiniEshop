import { Component, Inject, OnInit, Input, Output, EventEmitter, Predicate } from '@angular/core';
import { Observable } from 'rxjs';


export class Good {
    constructor(
        public id: string,
        public name: string,
        public price: number,
        public qty: number,
        public imageUrl: string,
        public categoryId: string
    ) { }
}


export class HeaderButton {
    @Output() click: EventEmitter<any>

    onClick(evt: any) {
        this.click.emit(evt);
    }

    constructor(
        public iconUrl: string,
        public label: string,
        public disablePredicate: Predicate<void>
    ) {
        this.click = new EventEmitter<any>();
    }
}

@Component({
    selector: 'blade-header',
    templateUrl: './blade-header.component.html',
    styleUrls: ['./blade-header.component.css']
})

export class BladeHeaderComponent  {
    @Input() bladeName: string;

    @Input() buttons: HeaderButton[];

    BladeHeaderComponent() {

    }
}

