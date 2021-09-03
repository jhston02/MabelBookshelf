<script>
    import {createEventDispatcher, onMount} from 'svelte';
    import {draw} from 'svelte/transition';
    import {chunk} from 'underscore';
    import {colorDictionary} from "$lib/common/util";

    let Carousel; // for saving Carousel component class
    let carousel; // for calling methods of carousel instance
    onMount(async () => {
        const module = await import('svelte-carousel');
        Carousel = module.default;
    });

    const dispatch = createEventDispatcher();

    const commands =
        "M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253";

    export let name;
    export let color;
    export let bookIds = [];
    export let categories;

    let show = true;
</script>

<div class="mx-auto md:m-0 rounded-2xl shadow-lg relative hover:shadow-2xl transition duration-500 {colorDictionary[color].mediumBackground} md:w-96 h-64 w-72 ">
    <div class="h-1/5 w-full flex justify-between flex-grow">
        <h1 class="{colorDictionary[color].darkText} p-2 pl-4 font-semibold tracking-tight truncate text-2xl">{name}</h1>
        <div class="active:bg-red-500 rounded-full {colorDictionary[color].darkText} active:text-white"
             on:click={() => dispatch('delete')}>
            <svg class="h-7 w-8 m-2 transform hover:scale-50 active:scale-105 transition duration-300 ease-in-out active:scale-100"
                 fill="none"
                 stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" stroke-linecap="round" stroke-linejoin="round"
                      stroke-width="2"/>
            </svg>
        </div>
    </div>
    <div class="{colorDictionary[color].lightBackground} h-4/5 w-full rounded-2xl flex justify-between items-center flex-wrap">
        <div class="flex flex-row flex-wrap justify-start w-full">
            {#each categories as category}
                <div class="transform hover:scale-110 flex justify-center items-center m-1 font-medium py-1 px-2 bg-white rounded-full {colorDictionary[colorDictionary[color].complementary].darkText} {colorDictionary[colorDictionary[color].complementary].lightBackground} border {colorDictionary[colorDictionary[color].complementary].border} ">
                    <div class="text-xs font-normal leading-none max-w-full flex-initial">{category}</div>
                </div>
            {/each}
        </div>
        {#if bookIds.length > 0}
            <svelte:component
                    this={Carousel}
                    bind:this={carousel}
            >
                {#each chunk(bookIds, 2) as bookChunk, chunkIndex (chunkIndex)}
                    <div class="flex flex-row justify-center md:justify-around w-full h-36">
                        {#each bookChunk as id (id)}
                            <img class="shadow-md  hover:shadow-1xl transform hover:scale-105 m-3"
                                 src="https://books.google.com/books?id={id}&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api"/>
                        {/each}
                    </div>
                {/each}
            </svelte:component>
        {:else}
            <div class="flex flex-row h-full" on:click={() => (show = !show)}>
                <div class=" h-full w-full absolute flex justify-center items-center">
                    {#if show}
                        <svg class="h-24 w-24" viewBox="0 0 24 24">
                            <path
                                    transition:draw={{duration: 1000}}
                                    d={commands}
                                    fill="none"
                                    stroke="black"
                                    stroke-width="0.1px"
                            />
                        </svg>
                    {/if}
                </div>

                <h1 class="font-extrabold text-2xl {colorDictionary[color].darkText} m-4">Add a book and let's get
                    started!</h1>
            </div>
        {/if}
    </div>
</div>