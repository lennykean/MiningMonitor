import { Component, OnInit } from '@angular/core';

import { Collector } from '../../../models/Collector';
import { CollectorService } from '../../collector.service';

@Component({
    templateUrl: './collectors.component.html'
})
export class CollectorsComponent implements OnInit {
    public collectors: Collector[];

    constructor(
        private collectorService: CollectorService) {
    }

    public async ngOnInit() {
        this.collectors = await this.collectorService.GetAll();
    }
}
